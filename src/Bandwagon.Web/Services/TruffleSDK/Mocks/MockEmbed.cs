namespace Bandwagon.Web.Services.TruffleSDK.Mocks;

public class MockEmbed : IEmbed
{
    public Task Hide()
    {
        return Task.CompletedTask;
    }
}
