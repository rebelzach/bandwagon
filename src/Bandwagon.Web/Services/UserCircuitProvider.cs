using System.Collections.Concurrent;
using Bandwagon.Web.Models.TruffleSDK;
using Bandwagon.Web.Services.TruffleSDK;

namespace Bandwagon.Web.Services;

public class UserCircuitProvider
{
    private readonly SharedCircuitRepository _circuitRepository;
    private readonly UserClient userClient;

    public UserCircuitProvider(
        SharedCircuitRepository circuitRepository,
        UserClient userClient)
    {
        this._circuitRepository = circuitRepository;
        this.userClient = userClient;
    }

    public async Task<UserCircuit> GetAsync()
    {
        var getfirstUser = new TaskCompletionSource<User>();
        await userClient.SubscribeUserAsync(user =>
        {
            if (!getfirstUser.Task.IsCompleted && user is not null)
            {
                getfirstUser.SetResult(user);
            }
        });
        var user = await getfirstUser.Task;

        if (user.Id is null)
            throw new NullReferenceException("User ID is null");

        var getfirstOrgUser = new TaskCompletionSource<OrgUser>();
        await userClient.SubscribeOrgUserAsync(orgUser =>
        {
            if (!getfirstOrgUser.Task.IsCompleted && orgUser is not null)
            {
                getfirstOrgUser.SetResult(orgUser);
            }
        });
        var orgUser = await getfirstOrgUser.Task;

        if (orgUser.Id is null)
            throw new NullReferenceException("Org User ID is null");

        var circuit = _circuitRepository.GetOrCreate<UserCircuit>(orgUser.Id);
        if (circuit.User is null)
        {
            circuit.User = user;
            circuit.OrgUser = orgUser;
        }
        return circuit;
    }
}
