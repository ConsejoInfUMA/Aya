using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Aya.Polls;

namespace Aya.Discord.Commands
{
    public class PollModule : ModuleBase<SocketCommandContext>
    {
        private readonly IPollService polls;

        public PollModule(IPollService polls)
        {
            this.polls = polls;
        }

        [Command("newpoll")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task NewPoll([Remainder] string title)
        {

            _ = Context.Message.DeleteAsync();

            var users = await Context.Channel.GetUsersAsync().FlattenAsync();
            users = users.Where(u => !u.IsBot);
            var voters = users.Select(u => u.Id);

            var msg = await Context.Channel.SendMessageAsync("Preparando urnas...");
            var poll = polls.StartPoll(title, msg.Id, Context.User.Id, voters);


            // we don't need to wait
            _ = UpdateMsg();
            _ = msg.AddReactionAsync(Constants.NextEmoji);
            _ = msg.PinAsync();
        }

        [Command("resend")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task Resend()
        {
            var msg = await Context.Channel.SendMessageAsync("...");
            polls.ChangeMessageId(msg.Id);
            _ = UpdateMsg();
        }


        [Command("join")]
        public Task JoinPoll([Remainder] string name)
        {
            polls.AddCandidate(Context.User.Id, name);
            Context.Message.AddReactionAsync(Constants.OkEmoji);
            return UpdateMsg();
        }

        private async Task UpdateMsg()
        {
            if (polls.TryGetActivePoll(out var poll))
            {
                var msg = (await Context.Channel.GetMessageAsync(poll.MessageId)) as IUserMessage;
                await msg.ModifyAsync(m => { m.Embed = poll.ToEmbed(); m.Content = ""; });
                _ = msg.AddReactionAsync(Constants.NextEmoji);
            }
        }

        [Command("startpoll")]
        [Alias("start", "pollstart", "registro")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task OpenCandidature()
        {
            polls.NextState();
            if (polls.TryGetActivePoll(out var poll))
            {
                await UpdateMsg();
            }
        }
    }
}

