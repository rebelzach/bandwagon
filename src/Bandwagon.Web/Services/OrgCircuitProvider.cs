using System.Collections.Concurrent;
using Bandwagon.Web.Models.TruffleSDK;
using Bandwagon.Web.Services.TruffleSDK;

namespace Bandwagon.Web.Services;

public class OrgCircuitProvider
{
    private readonly SharedCircuitRepository _circuitRepository;
    private readonly IOrgClient _orgClient;

    public OrgCircuitProvider(
        SharedCircuitRepository circuitRepository,
        IOrgClient orgClient)
    {
        this._circuitRepository = circuitRepository;
        this._orgClient = orgClient;
    }

    public async Task<OrgCircuit> GetAsync()
    {
        var getfirstOrg = new TaskCompletionSource<Org>();
        await _orgClient.SubscribeAsync(org =>
        {
            if (!getfirstOrg.Task.IsCompleted && org is not null)
            {
                getfirstOrg.SetResult(org);
            }
        });
        var org = await getfirstOrg.Task;

        if (org.Id is null)
            throw new NullReferenceException("Org ID is null");

        var circuit = _circuitRepository.GetOrCreate<OrgCircuit>(org.Id);
        if (circuit.Org is null)
        {
            circuit.Org = org;
        }
        return circuit;
    }
}
