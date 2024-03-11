using HBD.PulumiNet.Refits;
using Pulumi.AzureNative.Authorization;

namespace HBD.PulumiNet.AzAd;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class RoleAssignments
{
    public static RolesBuiltIn.AzRole GetRoleDefinitionByName(string roleName)
    {
        var role = RolesBuiltIn.Find(roleName);
        if (role == null) throw new Exception($"Role {roleName} not found");
        return role;
    }

    public record Args(string Name, string RoleName, Input<string> PrincipalId,
        PrincipalType PrincipalType, Input<string>? Scope = default, InputList<Resource>? DependsOn = default);

    public static RoleAssignment Create(Args args)
    {
        var scope = args.Scope ?? AzureEnv.DefaultScope;
        var role = GetRoleDefinitionByName(args.RoleName);

        return new RoleAssignment(
            $"{args.Name}-{args.RoleName.Replace(" ", string.Empty)}",
            new RoleAssignmentArgs
            {
                PrincipalId = args.PrincipalId,
                PrincipalType = args.PrincipalType,
                RoleDefinitionId = role.Id,
                Scope = scope,
            }, new CustomResourceOptions { DependsOn = args.DependsOn ?? [] });
    }
}