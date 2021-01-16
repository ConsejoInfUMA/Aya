using System.Threading.Tasks;
using Discord.Commands;

namespace Aya.Discord.Commands
{
    public class PollModule : ModuleBase<SocketCommandContext>
    {
        private readonly DiscordPolls _discordPolls;

        public PollModule(DiscordPolls discordPolls)
        {
            _discordPolls = discordPolls;
        }

        [Command("newpoll")]
        public Task NewPoll([Remainder] string title)
            => _discordPolls.NewPoll(Context, title);

    }
}

