using Bandwagon.Web.Models.TruffleSDK;

namespace Bandwagon.Web.Services;

public class OrgSession
{
    public OrgSession()
    {
    }

    public event Action<OrgSession>? SessionUpdated;

    private bool _groupingEnabled = true;

    public bool GroupingEnabled { 
        get => _groupingEnabled; 
        set
        {
            _groupingEnabled = value;
            SessionUpdated?.Invoke(this);
        }
    }

    private Org? _org;
    public Org? Org { 
        get => _org; 
        set
        {
            _org = value;
            SessionUpdated?.Invoke(this);
        }
    }
}
