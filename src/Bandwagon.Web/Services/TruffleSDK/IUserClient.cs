using Bandwagon.Web.Models.TruffleSDK;

namespace Bandwagon.Web.Services.TruffleSDK;
public interface IUserClient
{
    Task SubscribeOrgUserAsync(Action<OrgUser> user);
    Task SubscribeUserAsync(Action<User> user);
}