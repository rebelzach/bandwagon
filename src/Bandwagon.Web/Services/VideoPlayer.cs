using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bandwagon.Web.Services;

public class VideoPlayer : IAsyncDisposable
{
    private IJSObjectReference? _videoJs;
    private readonly IJSRuntime _js;

    public VideoPlayer(IJSRuntime js)
    {
        _js = js;
    }

    public async Task CreateTestVideoPlayerAsync(string elementId, IPlayback? playbackListener)
    {
        var videoJs = await GetOrCreateVideoJsAsync();
        if (playbackListener is not null)
        {
            // TODO: fix memory leak of listener
            var listener = DotNetObjectReference.Create(playbackListener);
            await videoJs.InvokeVoidAsync("createTestVideoPlayer", elementId, listener);
        }
        else
        {
            await videoJs.InvokeVoidAsync("createTestVideoPlayer", elementId);
        }
    }

    public async Task<double> GetCurrentTimeAsync()
    {
        var videoJs = await GetOrCreateVideoJsAsync();
        return await videoJs.InvokeAsync<double>("getCurrentTime");
    }

    private async Task<IJSObjectReference> GetOrCreateVideoJsAsync()
    {
        if (_videoJs is not null)
            return _videoJs;

        _videoJs = await _js.InvokeAsync<IJSObjectReference>("import", "/js-interop/video.js");
        return _videoJs;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_videoJs is not null)
            {
                await _videoJs.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // Do nothing
        }
    }
}
