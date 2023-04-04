using Bandwagon.Web.Models.TruffleSDK;

namespace Bandwagon.Web.Services.TruffleSDK;
public interface IOrgClient
{
    Task SubscribeAsync(Action<Org> org);
}