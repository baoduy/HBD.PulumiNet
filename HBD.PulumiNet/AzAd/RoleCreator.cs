using Pulumi.AzureAD;

namespace HBD.PulumiNet.AzAd;

public static class RoleCreator
{
    public record Args(
        Environments Env,
        string AppName,
        string Name)
    {
        public string? ModuleName { get; init; }
        public Input<string>[]? Members { get; init; }
        public GroupCreator.GroupPermissionArgs[]? Permissions { get; init; }
    }

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
        return GroupCreator.Create(new GroupCreator.Args(name)
        {
            Members = args.Members,
            Permissions = args.Permissions
        });
    }
}