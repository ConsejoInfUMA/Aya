using System.Threading.Tasks;

namespace Aya.Discord
{
    public interface ICommandHandler
    {
        Task InitializeAsync();
    }
}

