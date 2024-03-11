using Pulumi.AzureAD;

namespace HBD.PulumiNet.AzAd;

public static class RoleCreator
{
    public record Args(Environments Env,
        string AppName,
        string Name,
        string? ModuleName = default,
        Input<string>[]? Members = default,
        GroupCreator.GroupPermissionArgs[]? Permissions = null);

    public static string GetRoleName(Args args)
    {
        var e = args.Env == Environments.Prd ? "prod" : "non-prd";
        return string.IsNullOrEmpty(args.ModuleName)
            ? $"ROL {e} {args.AppName} {args.Name}".ToUpper()
            : $"ROL {e} {args.AppName}.{args.ModuleName} {args.Name}".ToUpper();
    }
    public static Group Create(Args args)
    {
        var name = GetRoleName(args);
        return GroupCreator.Create(new GroupCreator.Args(name,
            Members: args.Members,
            Permissions: args.Permissions));
    }
}