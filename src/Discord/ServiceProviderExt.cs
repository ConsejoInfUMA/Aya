using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace Aya.Discord
{
    public static class ServiceProviderExt
    {
        public static IServiceCollection AddDiscord(this IServiceCollection services) =>
          services.AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
          {
              AlwaysDownloadUsers = true,
              MessageCacheSize = 100
          }))
           .AddSingleton<DiscordPolls>()
           .AddSingleton<IClient, Client>()
           .AddSingleton<CommandService>()
           .AddSingleton<ICommandHandler, CommandHandler>();
    }
}

