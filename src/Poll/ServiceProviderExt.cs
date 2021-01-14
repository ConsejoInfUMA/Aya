using Aya.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Aya.Polls
{
    public static class ServiceProviderExt
    {
        public static IServiceCollection AddPoll(this IServiceCollection services) =>
           services
           .AddSingleton<IPollService, PollService>()
           .AddSingleton<IStorage<Poll>, PersistedStorage<Poll>>();
    }
}

