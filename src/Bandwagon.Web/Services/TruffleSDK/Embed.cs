using Microsoft.JSInterop;

namespace Bandwagon.Web.Services.TruffleSDK;

public class Embed
{
    private IJSObjectReference? _sdkModule;
    private readonly IJSRuntime _js;

    public Embed(IJSRuntime js)
    {
        _js = js;
    }

    public async Task Hide()
    {
        var sdk = await GetJsModuleAsync();
        await sdk.InvokeVoidAsync("embed.hide");
    }

    private async Task<IJSObjectReference> GetJsModuleAsync()
    {
        if (_sdkModule is not null)
            return _sdkModule;

        _sdkModule = await _js.InvokeAsync<IJSObjectReference>("import", "https://npm.tfl.dev/@trufflehq/sdk");
        return _sdkModule;
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_sdkModule is not null)
            {
                await _sdkModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // Do nothing
        }
    }
}
