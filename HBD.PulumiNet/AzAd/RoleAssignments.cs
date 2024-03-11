using HBD.PulumiNet.Refits;
using Pulumi.AzureNative.Authorization;

namespace HBD.PulumiNet.AzAd;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class RoleAssignments
{
    private static RolesBuiltIn.AzRole GetRoleDefinition(string roleName)
    {
        var role = RolesBuiltIn.Find(roleName);
        if (role == null) throw new Exception($"Role {roleName} not found");
        return role;
    }

    public record Args(
        string Name,
        string RoleName,
        Input<string> PrincipalId,
        PrincipalType PrincipalType,
        Input<string>? Scope = default,
        InputList<Resource>? DependsOn = default);

    public static RoleAssignment Assign(Args args)
    {
        var scope = args.Scope ?? AzureEnv.DefaultScope;
        var role = GetRoleDefinition(args.RoleName);

        return new RoleAssignment(
            $"{args.Name}-{args.RoleName.Replace(" ", string.Empty)}",
            new RoleAssignmentArgs
            {
                PrincipalId = args.PrincipalId,
                PrincipalType = args.PrincipalType,
                RoleDefinitionId = role.Scope,
                Scope = scope,
            }, new CustomResourceOptions { DependsOn = args.DependsOn ?? [] });
    }

    public record GroupRoleArgs(
        string Name,
        Input<string> GroupId,
        Input<string> Scope,
        InputList<Resource>? DependsOn = default);

    public static void AssignToGroup(GroupRoleArgs args, params string[] roleNames)
    {
        foreach (var r in roleNames)
            Assign(new Args(args.Name, r, args.GroupId, PrincipalType.Group, Scope: args.Scope));
    }
}