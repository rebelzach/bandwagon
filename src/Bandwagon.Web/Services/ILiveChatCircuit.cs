using Bandwagon.Web.Models;

namespace Bandwagon.Web.Services;
public interface ILiveChatCircuit : ISharedCircuit
{
    event Action<StoredChatMessage>? ChatMessageRecieved;
}
