using Bandwagon.Web.Models;
using Bandwagon.Web.Models.TruffleSDK;
using Microsoft.JSInterop;

namespace Bandwagon.Web.Services;

public class SavedLiveStream : ILiveStream, IDisposable
{
    private const string ChatTranscriptPath = "";

    public SavedLiveStream()
    {
    }

    public event Action<StoredChatMessage>? ChatMessageRecieved;

    public void EnsureLoaded()
    {

    }

    public void Play(double positionSeconds)
    {

    }

    public void Pause()
    {

    }

    public void Dispose()
    {
    }
}
