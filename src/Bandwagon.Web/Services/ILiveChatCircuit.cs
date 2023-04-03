using Bandwagon.Web.Models;

namespace Bandwagon.Web.Services;
public interface ILiveChatCircuit : ISharedCircuit
{
    GroupedMessageCollector Messages { get; }

    event Action? Updated;
}
