using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Aya.Discord
{
    public class CommandHandler : ICommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly IServiceProvider _services;
        private readonly CommandService _commands;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage msg)
        {
            var message = msg as SocketUserMessage;
            if (message is null || message.Author.IsBot)
            {
                return;
            }

            int argPos = 0;

            if (!(message.HasMentionPrefix(_client.CurrentUser, ref argPos) ||
                message.Author.IsBot))
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            var res = await _commands.ExecuteAsync(context, argPos, _services);
            if (!res.IsSuccess)
            {
                await context.Channel.SendMessageAsync(res.ErrorReason);
            }
        }
    }
}

