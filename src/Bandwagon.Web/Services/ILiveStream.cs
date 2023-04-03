using Bandwagon.Web.Models;

namespace Bandwagon.Web.Services;
public interface ILiveStream
{
    event Action<StoredChatMessage>? ChatMessageRecieved;
}
