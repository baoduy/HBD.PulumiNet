using Pulumi.AzureAD;

namespace HBD.PulumiNet.AzAd;

public static class EnvRoles
{
    private record EnvRole(Environments Env, string RoleName, string AppName);

    public record EnvRoleResults(string ReadOnly, string Contributor, string Admin);

    private static (EnvRole readOnly, EnvRole contributor, EnvRole admin) GetInternalEnvRoles(Environments env)
    {
        var readOnly = new EnvRole(env, "ReadOnly", "Azure");
        var contributor = new EnvRole(env, "Contributor", "Azure");
        var admin = new EnvRole(env, "Admin", "Azure");
        return (readOnly, contributor, admin);
    }

    public static EnvRoleResults GetEnvRoles(Environments env)
    {
        var (readOnly, contributor, admin) = GetInternalEnvRoles(env);
        
        return new EnvRoleResults(
            ReadOnly: RoleCreator.GetRoleName(
                new RoleCreator.Args(readOnly.Env, readOnly.AppName, readOnly.RoleName)),
            Contributor: RoleCreator.GetRoleName(
                new RoleCreator.Args(contributor.Env, contributor.AppName, contributor.RoleName)),
            Admin: RoleCreator.GetRoleName(
                new RoleCreator.Args(admin.Env, admin.AppName, admin.RoleName))
        );
    }

    public static (Group readonlyGroup, Group contributorGroup, Group adminGroup) CreateEnvRoles(Environments env)
    {
        var (readOnly, contributor, admin)  = GetInternalEnvRoles(env);

        var readonlyGroup = RoleCreator.Create(
            new RoleCreator.Args(readOnly.Env, readOnly.AppName, readOnly.RoleName));
        var contributorGroup = RoleCreator.Create(
            new RoleCreator.Args(contributor.Env, contributor.AppName, contributor.RoleName));
        var adminGroup = RoleCreator.Create(
            new RoleCreator.Args(admin.Env, admin.AppName, admin.RoleName));

        return (readonlyGroup, contributorGroup, adminGroup);
    }
}