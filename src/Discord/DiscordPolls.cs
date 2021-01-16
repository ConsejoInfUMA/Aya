using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Aya.Polls;
using Discord.Commands;
using System.Linq;
using System.Collections.Generic;
using System;
using Aya.Logger;

namespace Aya.Discord
{
    public class DiscordPolls
    {
        private readonly DiscordSocketClient _client;
        private readonly IPollService _polls;
        private readonly ILogger _logger;

        private IUserMessage _message;
        private SocketGuild _guild;

        private HashSet<ulong> _voted = new HashSet<ulong>();

        public DiscordPolls(DiscordSocketClient client, IPollService polls, ILogger logger)
        {
            _polls = polls;
            _logger = logger;
            _client = client;
            _polls.OnPollUpdated += OnPollUpdated;
            _polls.OnPollStateUpdated += OnPollStateUpdated;
        }

        private bool CanVote(ulong id)
            => _polls.ActivePoll.Voters.TryGetValue(id, out bool didVote) && !didVote;

        private async Task OnMessageReceived(SocketMessage msg)
        {
            if (msg.Author.IsBot)
            {
                return;
            }

            var usrId = msg.Author.Id;
            if (!CanVote(usrId))
            {
                _logger.LogError($"User {usrId} - {msg.Author.Username} can't vote");
                return;
            }


            if (msg.Channel is SocketDMChannel dmChannel)
            {
                await msg.AddReactionAsync(Constants.OkEmoji);
                List<Vote> res = ProcessVote(msg.Content);
                var formatted = String.Join("\n", res.Select(v => $"{v.Value} puntos para {v.Candidate.Name}"));
                if (String.IsNullOrEmpty(formatted))
                {
                    formatted = "Invalid input";
                }
                await dmChannel.SendMessageAsync(formatted);
            }
        }

        private List<Vote> ProcessVote(string content)
        {

            // Allow only digits and commas
            var txt = new string(content.Where(c => Char.IsDigit(c) || c == ',').ToArray());
            var splitted = txt.Split(',');

            List<Vote> votes = new List<Vote>();
            int points = 15;
            foreach (var vote in splitted)
            {
                if (ulong.TryParse(vote, out var result))
                {
                    var candidate = _polls.ActivePoll.Candidates.FirstOrDefault(c => c.Id == result && votes.All(v => v.Candidate.Id != result));

                    if (candidate != null)
                    {
                        votes.Add(new Vote
                        {
                            Candidate = candidate,
                            Value = points
                        });
                    }
                }

                points--;
                if (points <= 0)
                {
                    break;
                }
            }
            return votes;
        }

        private void OnPollStateUpdated(Poll poll)
        {
            if (poll.State == PollState.Registering)
            {
                _message.AddReactionAsync(Constants.JoinEmoji);
            }
            else if (poll.State == PollState.SendingMessages)
            {
                _client.MessageReceived += OnMessageReceived;
                _client.ReactionRemoved += OnReactionRemoved;
                _ = SendMessages(poll);
            }
            else if (poll.State == PollState.Voting)
            {
                _client.ReactionRemoved -= OnReactionRemoved;
            }
            else if (poll.State == PollState.Finished)
            {
                _client.MessageReceived -= OnMessageReceived;
            }

            _logger.LogInfo($"Poll state updated: {poll}");
        }

        private async Task SendMessages(Poll poll)
        {
            var candidates = String.Join("\n", poll.Candidates.Select(c => $"{c.Id} - {c.Name}"));
            var message = $"**{poll.Title}**\n" +
                $"Para votar, deberás escribir una lista separada por comas indicando el identificador de cada candidato. \n**Candidatos:**\n{candidates}";

            foreach (var voter in poll.Voters)
            {
                var user = _guild.GetUser(voter.Key);
                // avoid rate limit
                await Task.Delay(900);
                await user.SendMessageAsync(message);
            }

            _polls.NextState();
        }

        private void OnPollUpdated(Poll poll)
        {
            _ = Update(poll);
        }

        internal async Task NewPoll(SocketCommandContext context, string title)
        {
            _ = context.Message.DeleteAsync();
            if (!(context.User as IGuildUser).RoleIds.Contains(Constants.ModRole))
            {
                return;
            }

            var users = await (context.Channel as IGuildChannel).GetUsersAsync().FlattenAsync();
            users = users.Where(u => !u.IsBot && u.RoleIds.Contains(Constants.VoterRole));
            var voters = users.Select(u => u.Id);

            _message = await context.Channel.SendMessageAsync("Cargando...");
            _guild = context.Guild;
            _polls.StartPoll(title, _message.Id, context.User.Id, voters);

            _client.ReactionAdded += OnReact;

            _ = _message.AddReactionAsync(Constants.NextEmoji);
        }

        private async Task Update(Poll poll)
        {
            await _message.ModifyAsync(m => { m.Embed = poll.ToEmbed(); m.Content = ""; });
        }


        internal Task AddCandidate(SocketCommandContext context, string name)
        {
            _polls.AddCandidate(context.User.Id, name);
            return context.Message.AddReactionAsync(Constants.OkEmoji);
        }


        private async Task OnReact(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.UserId == _client.CurrentUser.Id)
            {
                return;
            }

            if (_polls.ActivePoll == null)
            {
                _logger.LogError($"Received a reaction when there isnt an active poll");
                return;
            }


            // If the reaction comes from a guild
            if (channel is IGuildChannel guildChannel)
            {
                var msg = await reaction.Channel.GetMessageAsync(reaction.MessageId) as IUserMessage;
                if (msg is null)
                {
                    return;
                }

                var usr = await reaction.Channel.GetUserAsync(reaction.UserId) as IGuildUser;
                if (usr is null)
                {
                    return;
                }

                OnGuildReaction(reaction, msg, usr);
            }
            else if (_polls.ActivePoll.State == PollState.Voting && channel is SocketDMChannel dmChannel)
            {
                var msg = reaction.Message.GetValueOrDefault() ??
                    await reaction.Channel.GetMessageAsync(reaction.MessageId);

                OnDmReaction(reaction, msg, dmChannel);
            }

        }

        private async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.UserId == _client.CurrentUser.Id)
            {
                return;
            }

            if (_polls.ActivePoll == null)
            {
                _logger.LogError($"Received a reaction when there isnt an active poll");
                return;
            }


            // If the reaction comes from a guild
            if (channel is IGuildChannel guildChannel)
            {
                var msg = await reaction.Channel.GetMessageAsync(reaction.MessageId) as IUserMessage;
                if (msg is null)
                {
                    return;
                }

                var usr = await reaction.Channel.GetUserAsync(reaction.UserId) as IGuildUser;
                if (usr is null)
                {
                    return;
                }

                OnGuildReactionRemoved(reaction, msg, usr);

            }
        }

        private void OnGuildReactionRemoved(SocketReaction reaction, IUserMessage msg, IGuildUser usr)
        {
            if (_polls.ActivePoll.State == PollState.Registering && reaction.Emote.Name == Constants.JoinEmoji.Name)
            {
                // TODO: check if user has consejo role
                if (!usr.RoleIds.Contains(Constants.VoterRole))
                {
                    return;
                }

                if (_polls.RemoveCandidate(usr.Id))
                {
                    var name = usr.Nickname ?? usr.Username;
                    _logger.LogInfo($"Candidate removed for poll {_polls.ActivePoll.Id}: {name} with id {usr.Id}");
                }


            }

        }

        private void OnDmReaction(SocketReaction reaction, IMessage msg, SocketDMChannel dmChannel)
        {
            // Check if user can vote


            var usrId = reaction.UserId;
            if (!CanVote(usrId))
            {
                _logger.LogError($"User {usrId} - {msg.Author.Username} can't vote");
                return;
            }

            if (reaction.Emote.Name == Constants.OkEmoji.Name)
            {
                var result = ProcessVote(msg.Content);
                _polls.SendVote(result, usrId);
                dmChannel.SendMessageAsync("Voto guardado");
            }
        }

        private void OnGuildReaction(SocketReaction reaction, IUserMessage msg, IGuildUser usr)
        {
            if (msg.Id != _polls.ActivePoll?.MessageId)
            {
                return;
            }

            if (_polls.ActivePoll.State == PollState.Registering && reaction.Emote.Name == Constants.JoinEmoji.Name && usr.RoleIds.Contains(Constants.VoterRole))
            {
                // avoid updating
                if (_polls.ActivePoll.Candidates.Any(c => c.Id == usr.Id))
                {
                    return;
                }

                var name = usr.Nickname ?? usr.Username;

                _logger.LogInfo($"New candidate for poll {_polls.ActivePoll.Id}: {name} with id {usr.Id}");

                _polls.AddCandidate(usr.Id, name);
            }
            else if (reaction.Emote.Name == Constants.NextEmoji.Name && usr.RoleIds.Contains(Constants.ModRole))
            {
                _polls.NextState();
            }
        }
    }
}

