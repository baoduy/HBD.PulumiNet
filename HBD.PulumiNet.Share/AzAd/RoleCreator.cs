using HBD.PulumiNet.Share.Common;
using Pulumi;
using Pulumi.AzureAD;

namespace HBD.PulumiNet.Share.AzAd;

public static class RoleCreator
{
    public record Args(Environments Env,
        string AppName,
        string Name,
        string? ModuleName = default,
        string Location = "GLB",
        Input<string>[]? Members = default,
        GroupCreator.GroupPermissionArgs[]? Permissions = null);

    public static Task<Group> Create(Args args)
    {
        var e = args.Env == Environments.Prd ? "prod" : "non-prd";

        var name = string.IsNullOrEmpty(args.ModuleName)
            ? $"ROL {e} {args.Location} {args.AppName} {args.Name}"
            : $"ROL {e} {args.Location} {args.AppName}.{args.ModuleName} {args.Name}";

        return GroupCreator.Create(new GroupCreator.Args(name,
            Members: args.Members,
            Permissions: args.Permissions));
    }
}