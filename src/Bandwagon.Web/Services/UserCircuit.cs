using Bandwagon.Web.Models.TruffleSDK;

namespace Bandwagon.Web.Services;

public class UserCircuit : ISharedCircuit
{
    public UserCircuit()
    {
    }

    public event Action<UserCircuit>? Updated;

    private bool _showGroupingOnVideo = false;
    public bool ShowGroupingOnVideo { 
        get => _showGroupingOnVideo; 
        set
        {
            _showGroupingOnVideo = value;
            Updated?.Invoke(this);
        }
    }

    private bool _isAdmin = true;
    public bool IsOrgAdmin { 
        get => _isAdmin; 
        set
        {
            _isAdmin = value;
            Updated?.Invoke(this);
        }
    }

    private User? _user;
    public User? User { 
        get => _user; 
        set
        {
            _user = value;
            Updated?.Invoke(this);
        }
    }

    private OrgUser? _orgUser;
    public OrgUser? OrgUser { 
        get => _orgUser; 
        set
        {
            _orgUser = value;
            Updated?.Invoke(this);
        }
    }
}
