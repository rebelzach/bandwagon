using Bandwagon.Web.Models.TruffleSDK;

namespace Bandwagon.Web.Services;

public class OrgCircuit : ISharedCircuit
{
    public OrgCircuit()
    {
    }

    public event Action<OrgCircuit>? Updated;

    private bool _isDanceParty = false;
    public bool IsDanceParty { 
        get => _isDanceParty; 
        set
        {
            _isDanceParty = value;
            Updated?.Invoke(this);
        }
    }

    private bool _groupingEnabled = true;
    public bool GroupingEnabled { 
        get => _groupingEnabled; 
        set
        {
            _groupingEnabled = value;
            Updated?.Invoke(this);
        }
    }

    private Org? _org;
    public Org? Org { 
        get => _org; 
        set
        {
            _org = value;
            Updated?.Invoke(this);
        }
    }
}
