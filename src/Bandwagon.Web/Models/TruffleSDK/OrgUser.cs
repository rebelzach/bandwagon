namespace Bandwagon.Web.Models.TruffleSDK;

public sealed class OrgUser
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public Connection<Role>? RoleConnection { get; set; }

    //public Connection? ActivePowerupConnection { get; set; }
}

public static class OrgUserExtensions
{
    public static bool? IsAdmin(this OrgUser orgUser)
    {
        return orgUser.RoleConnection?.Nodes?.Any(n => n.Slug == "admin");
    }
}

public sealed class Role
{
    public string? Id { get; set; }

    /// <summary>
    /// Known slugs: admin, everyone
    /// </summary>
    public string? Slug { get; set; }

    public string? Name { get; set; }

    //public Connection<Permission>? PermissionConnection { get; set; }
}

public sealed class Connection<T>
{
    public T[]? Nodes { get; set; }
}
