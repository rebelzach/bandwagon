using Bandwagon.Web.Models;
using Bandwagon.Web.Models.TruffleSDK;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Bandwagon.Web.Services;

public class PrerecordedLiveChatCircuit : ILiveChatCircuit, IDisposable, IPlayback
{
    private const string ChatTranscriptPath = @"C:\Temp\chat5.json";

    private readonly GroupedMessageCollector _messageCollector;

    private IEnumerable<IChatMessage> _messages = Enumerable.Empty<IChatMessage>();
    private long startTimeMs = 0;
    private long postitionMs = 0;
    private Task? _runLoopTask;
    private long endTimeMs = 0;
    private bool _isLoaded = false;
    private bool _isPaused;
    private bool _isReloadQueued;
    private DateTime? _lastTick;

    public PrerecordedLiveChatCircuit(GroupedMessageCollector messageCollector)
    {
        _messageCollector = messageCollector;
    }

    public event Action? Updated;

    public GroupedMessageCollector Messages => _messageCollector;

    public async Task EnsureLoadedAsync()
    {
        if (_isLoaded) 
            return;

        _isLoaded = true;

        var json = await File.ReadAllTextAsync(ChatTranscriptPath);
        _messages = JsonConvert.DeserializeObject<List<StoredChatMessage>>(json) 
            ?? throw new NullReferenceException();

        //startTime = messages.Where(m => m is not null).MinBy(m => m.TmiSentTs)!.TmiSentTs; 
        // "Message": "just like a DND party pcrowGiggle",
        // "TmiSentTs": "1676325886126",
        // VOD: 01:02:10
        //@*chat1 timestamp = 1675383350799; *@
        //timestamp = startTime;

        startTimeMs = 1676325886126L - (long)TimeSpan.Parse("01:02:10.500").TotalMilliseconds + 8000L;
        endTimeMs = _messages.Where(m => m is not null).MaxBy(m => m.TmiSentTs)!.TmiSentTs;

        postitionMs = startTimeMs;

        _runLoopTask = RunLoop();
    }

    private async Task RunLoop()
    {
        while (true)
        {
            foreach (var message in _messages)
            {
                await CheckPauseAsync();

                var tooOldTimestamp = postitionMs - (30 * 1000);
                if (message.TmiSentTs < tooOldTimestamp)
                {
                    continue;
                }

                while (message.TmiSentTs > postitionMs)
                {
                    await CheckPauseAsync();
                    if (_isReloadQueued)
                    {
                        break;
                    }
                    await Task.Delay(50);
                    UpdateTimestamp();
                }

                if (_isReloadQueued)
                {
                    _isReloadQueued = false;
                    break;
                }

                _messageCollector.ReceiveMessage(message);
                Updated?.Invoke();
            }
        }
    }

    private async Task CheckPauseAsync()
    {
        while (_isPaused)
        {
            await Task.Delay(50);
            await Task.Yield();
        }
    }

    private void UpdateTimestamp()
    {
        var now = DateTime.Now;

        _lastTick ??= now;

        postitionMs += (long)(now - _lastTick.Value).TotalMilliseconds;

        _lastTick = now;
    }

    [JSInvokable]
    public void SetPosition(double positionSeconds)
    {
        postitionMs = startTimeMs + (long)(positionSeconds * 1000.0);
        _lastTick = DateTime.Now;
        _messageCollector.Reset();
        _isReloadQueued = true;
    }

    [JSInvokable]
    public void Play()
    {
        if (!_isPaused)
            return;

        _lastTick = DateTime.Now;
        _isPaused = false;
    }

    [JSInvokable]
    public void Pause()
    {
        if (_isPaused)
            return;

        _lastTick = null;
        _isPaused = true;
    }

    public void Dispose()
    {
    }
}
