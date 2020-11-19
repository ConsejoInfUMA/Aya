using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Aya.Config;
using Aya.Discord;

namespace Aya
{
    public class Services
    {
        public ServiceProvider Build() =>
           new ServiceCollection()
           .AddSingleton<IConfigProvider, ConfigProvider>()
           .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
           {
               AlwaysDownloadUsers = true,
               MessageCacheSize = 100
           }))
           .AddSingleton<IClient, Client>()
           .AddSingleton<CommandService>()
           .AddSingleton<ICommandHandler, CommandHandler>()
           .BuildServiceProvider();
    }
}

