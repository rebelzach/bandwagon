using Bandwagon.Web.Models.TruffleSDK;

namespace Bandwagon.Web.Services;

public class UserSession
{
    public UserSession()
    {
    }

    public event Action<UserSession>? SessionUpdated;

    private bool _showGrouping = true;

    public bool ShowGrouping { 
        get => _showGrouping; 
        set
        {
            _showGrouping = value;
            SessionUpdated?.Invoke(this);
        }
    }

    private User? _user;
    public User? User { 
        get => _user; 
        set
        {
            _user = value;
            SessionUpdated?.Invoke(this);
        }
    }

    private OrgUser? _orgUser;
    public OrgUser? OrgUser { 
        get => _orgUser; 
        set
        {
            _orgUser = value;
            SessionUpdated?.Invoke(this);
        }
    }
}
