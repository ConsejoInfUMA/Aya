using Microsoft.Extensions.DependencyInjection;
using Aya.Discord;
using Aya.Config;
using Aya.Polls;
using Aya.Logger;

namespace Aya
{
    public class Services
    {
        public ServiceProvider Build() =>
           new ServiceCollection()
           .AddDiscord()
           .AddPoll()
           .AddSingleton<ILogger, ConsoleLogger>()
           .AddSingleton<IConfigProvider, ConfigProvider>()
           .BuildServiceProvider();
    }
}

