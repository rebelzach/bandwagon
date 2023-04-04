using Bandwagon.Web.Models.TruffleSDK;
using Bandwagon.Web.Services.TruffleSDK;

namespace Bandwagon.Web.Services;

public class LiveChatCircuitProvider
{
    private readonly SharedCircuitRepository _circuitRepository;
    private readonly IOrgClient _orgClient;

    public LiveChatCircuitProvider(
        SharedCircuitRepository circuitRepository,
        IOrgClient orgClient)
    {
        this._circuitRepository = circuitRepository;
        this._orgClient = orgClient;
    }

    public async Task<ILiveChatCircuit> GetAsync()
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

        // For now use the org ID to lookup the chat circuit.
        var circuit = _circuitRepository.GetOrCreate<ILiveChatCircuit>(org.Id);
        return circuit;
    }
}
