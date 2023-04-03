using Bandwagon.Web.Models;
using Bandwagon.Web.Models.TruffleSDK;
using Microsoft.JSInterop;

namespace Bandwagon.Web.Services.TruffleSDK;

public class UserClient
{
    private IJSObjectReference? _orgHelper;
    private DotNetObjectReference<UserClient>? _thisRef;
    private readonly IJSRuntime _js;
    private bool _isUserSubscribed;
    private bool _isOrgUserSubscribed;

    public UserClient(IJSRuntime js)
    {
        _js = js;
    }

    private event Action<User>? _nextUser;
    private event Action<OrgUser>? _nextOrgUser;

    public async Task SubscribeUserAsync(Action<User> user)
    {
        if (!_isUserSubscribed)
        {
            _isUserSubscribed = true;
            _orgHelper = await _js.InvokeAsync<IJSObjectReference>("import", "/js-interop/user-helper.js");
            _thisRef = DotNetObjectReference.Create(this);
            await _orgHelper.InvokeVoidAsync("subscribeUser", _thisRef);
        }
        _nextUser += user;
    }

    public async Task SubscribeOrgUserAsync(Action<OrgUser> user)
    {
        if (!_isOrgUserSubscribed)
        {
            _isOrgUserSubscribed = true;
            _orgHelper = await _js.InvokeAsync<IJSObjectReference>("import", "/js-interop/user-helper.js");
            _thisRef = DotNetObjectReference.Create(this);
            await _orgHelper.InvokeVoidAsync("subscribeOrgUser", _thisRef);
        }
        _nextOrgUser += user;
    }

    [JSInvokable]
    public void NextUser(User? user)
    {
        if (user is null)
            return;

        _nextUser?.Invoke(user);
    }

    [JSInvokable]
    public void NextOrgUser(OrgUser? user)
    {
        if (user is null)
            return;

        _nextOrgUser?.Invoke(user);
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (_orgHelper is not null)
            {
                await _orgHelper.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // Do nothing
        }
    }
}
