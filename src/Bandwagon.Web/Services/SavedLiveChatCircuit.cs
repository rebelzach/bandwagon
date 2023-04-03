using Bandwagon.Web.Models;
using Bandwagon.Web.Models.TruffleSDK;
using Microsoft.JSInterop;

namespace Bandwagon.Web.Services;

public class PrerecordedLiveChatCircuit : ILiveChatCircuit, IDisposable, IPlayback
{
    private const string ChatTranscriptPath = "";

    public PrerecordedLiveChatCircuit()
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
