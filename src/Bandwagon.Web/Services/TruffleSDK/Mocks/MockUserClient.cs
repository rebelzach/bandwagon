using Bandwagon.Web.Models.TruffleSDK;

namespace Bandwagon.Web.Services.TruffleSDK.Mocks;

public class MockUserClient : IUserClient
{
    public Task SubscribeOrgUserAsync(Action<OrgUser> user)
    {
        user(new OrgUser()
        {
            Id = "TESTUSER",
            Name = "Test User",
            RoleConnection = new Connection<Role>()
            {
                Nodes = new[] 
                {
                    new Role()
                    {
                        Id = "ADMINROLE",
                        Slug = "admin",
                        Name = "Admin Role"
                    }
                }
            }
        });
        return Task.CompletedTask;
    }

    public Task SubscribeUserAsync(Action<User> user)
    {
        user(new User()
        {
            Id = "TESTUSER",
            Name = "Test User",
        });
        return Task.CompletedTask;
    }
}
