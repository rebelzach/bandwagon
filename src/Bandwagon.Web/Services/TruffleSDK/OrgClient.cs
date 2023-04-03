using Bandwagon.Web.Models.TruffleSDK;
using Microsoft.JSInterop;

namespace Bandwagon.Web.Services.TruffleSDK;

public class OrgClient
{
    private IJSObjectReference? _orgHelper;
    private DotNetObjectReference<OrgClient>? _thisRef;
    private readonly IJSRuntime _js;
    private bool _isSubscribed;

    public OrgClient(IJSRuntime js)
    {
        _js = js;
    }

    private event Action<Org>? _nextOrg;

    public async Task SubscribeAsync(Action<Org> org)
    {
        if (!_isSubscribed)
        {
           _isSubscribed = true;
            _orgHelper = await _js.InvokeAsync<IJSObjectReference>("import", "/js-interop/org-helper.js");
            _thisRef = DotNetObjectReference.Create(this);
            await _orgHelper.InvokeVoidAsync("subscribe", _thisRef);
        }
        _nextOrg += org;
    }

    [JSInvokable]
    public void NextOrg(Org? org)
    {
        if (org is null)
            return;

        _nextOrg?.Invoke(org);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_orgHelper is not null)
            {
                await _orgHelper.DisposeAsync();
            }

            if (_thisRef is not null)
            {
                _thisRef.Dispose();
            }
        }
        catch (JSDisconnectedException)
        {
            // Do nothing
        }
    }
}
