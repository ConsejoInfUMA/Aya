using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Aya.Config;

namespace Aya.Discord
{
    public class Client : IClient
    {
        private readonly IConfigProvider _config;
        private readonly DiscordSocketClient _client;
        private readonly ICommandHandler _commands;

        public Client(IConfigProvider configProvider, DiscordSocketClient client, ICommandHandler commands)
        {
            _config = configProvider;
            _client = client;
            _commands = commands;
        }

        public async Task StartAsync()
        {
            var config = _config.ReadConfig();
            if (config == null || !config.IsInitialized())
            {
                await Log("Please add your bot token to the config.json file");
                return;
            }

            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, _config.ReadConfig().Token);
            await _client.StartAsync();
            await _commands.InitializeAsync();
            await Task.Delay(-1);
        }

        private Task Log(string msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

        private Task Log(LogMessage msg)
        {
            Log(msg.ToString());
            return Task.CompletedTask;
        }
    }
}

