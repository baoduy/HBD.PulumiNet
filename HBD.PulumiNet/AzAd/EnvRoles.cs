namespace HBD.PulumiNet.AzAd;

public static class EnvRoles
{
    private record EnvRole(Environments Env, string RoleName, string AppName);

    private record InternalEnvRoleResults(EnvRole ReadOnly, EnvRole Contributor, EnvRole Admin);

    public record EnvRoleResults(string ReadOnly, string Contributor, string Admin);

    private static InternalEnvRoleResults GetInternalEnvRoles(Environments env) => new(
        new EnvRole(env, "ReadOnly", "Azure"),
        new EnvRole(env, "Contributor", "Azure"),
        new EnvRole(env, "Admin", "Azure")
    );

    public static EnvRoleResults GetEnvRoles(Environments env)
    {
        var internalRole = GetInternalEnvRoles(env);
        return new EnvRoleResults(
            ReadOnly: RoleCreator.GetRoleName(
                new RoleCreator.Args(internalRole.ReadOnly.Env, internalRole.ReadOnly.AppName,
                    internalRole.ReadOnly.RoleName)),
            Contributor: RoleCreator.GetRoleName(
                new RoleCreator.Args(internalRole.Contributor.Env, internalRole.Contributor.AppName,
                    internalRole.Contributor.RoleName)),
            Admin: RoleCreator.GetRoleName(
                new RoleCreator.Args(internalRole.Admin.Env, internalRole.Admin.AppName,
                    internalRole.Admin.RoleName))
        );
    }

    public static void CreateEnvRoles(Environments env)
    {
        var internalRole = GetInternalEnvRoles(env);
        var readonlyGroup = RoleCreator.Create(
            new RoleCreator.Args(internalRole.ReadOnly.Env, internalRole.ReadOnly.AppName,
                internalRole.ReadOnly.RoleName));
        var contributorGroup =RoleCreator.Create(
            new RoleCreator.Args(internalRole.Contributor.Env, internalRole.Contributor.AppName,
                internalRole.Contributor.RoleName));
        var adminGroup =RoleCreator.Create(
            new RoleCreator.Args(internalRole.Admin.Env, internalRole.Admin.AppName,
                internalRole.Admin.RoleName));
    }
}