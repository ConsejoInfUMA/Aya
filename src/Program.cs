using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Aya.Discord;

namespace Aya
{
    public class Program
    {
        private Services _services;

        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _services = new Services();
            using (var services = _services.Build())
            {
                var client = services.GetRequiredService<IClient>();
                await client.StartAsync();
            }

        }
    }
}

