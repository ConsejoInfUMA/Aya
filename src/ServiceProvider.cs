using Microsoft.Extensions.DependencyInjection;
using Aya.Discord;
using Aya.Config;
using Aya.Polls;

namespace Aya
{
    public class Services
    {
        public ServiceProvider Build() =>
           new ServiceCollection()
           .AddDiscord()
           .AddPoll()
           .AddSingleton<IConfigProvider, ConfigProvider>()
           .BuildServiceProvider();
    }
}

