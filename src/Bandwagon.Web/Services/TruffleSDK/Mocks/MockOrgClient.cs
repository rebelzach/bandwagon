using Bandwagon.Web.Models.TruffleSDK;

namespace Bandwagon.Web.Services.TruffleSDK.Mocks;

public class MockOrgClient : IOrgClient
{
    public Task SubscribeAsync(Action<Org> org)
    {
        org(new Org()
        {
            Id = "TESTORG",
            Name = "Test Org",
        });
        return Task.CompletedTask;
    }
}
