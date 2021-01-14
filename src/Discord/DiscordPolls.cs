using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Aya.Polls;

namespace Aya.Discord
{
    public class DiscordPolls
    {
        private readonly DiscordSocketClient _client;
        private readonly IPollService _polls;

        public DiscordPolls(DiscordSocketClient client, IPollService polls)
        {
            _polls = polls;
            _client = client;
            _client.ReactionAdded += OnReact;
            System.Console.WriteLine("Discord polls module started");
        }

        public void Initialize()
        {

        }

        private async Task OnReact(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var chn = channel as IGuildChannel;

            // not a guild channel
            if (channel is null)
            {
                return;
            }

            var msg = await reaction.Channel.GetMessageAsync(reaction.MessageId) as IUserMessage;
            var usr = await reaction.Channel.GetUserAsync(reaction.UserId) as IGuildUser;

            if (
                msg is null ||
                usr is null ||
                reaction.UserId == _client.CurrentUser.Id
                )
            {
                return;
            }

            if (reaction.Emote.Name == Constants.NextEmoji.Name &&
                (usr as IGuildUser).GuildPermissions.Has(GuildPermission.Administrator))
            {
                if (_polls.TryGetActivePoll(out var poll))
                {
                    _polls.NextState();
                    await msg.RemoveAllReactionsAsync();
                    _ = msg.ModifyAsync(m => m.Embed = poll.ToEmbed());
                    _ = msg.AddReactionAsync(reaction.Emote);
                }
            }
        }
    }
}

