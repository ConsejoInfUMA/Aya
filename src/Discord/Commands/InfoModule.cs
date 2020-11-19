using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Aya.Discord.Commands
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public Task Ping()
            => ReplyAsync("Pong!");
    }
}

