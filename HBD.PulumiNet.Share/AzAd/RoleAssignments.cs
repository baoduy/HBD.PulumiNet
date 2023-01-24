using HBD.PulumiNet.Share.Common;
using HBD.PulumiNet.Share.Refits;
using Pulumi;
using Pulumi.AzureNative.Authorization;

namespace HBD.PulumiNet.Share.AzAd;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class RoleAssignments
{
    public static async Task<RoleDefinitionResult> GetRoleDefinitionByName(string roleName)
    {
        var client = await Factory.Create<IAzureAd>();
        var rs = await client.GetRoleDefinitionAsync(roleName);

        var role = rs.Value.FirstOrDefault();
        if (role == null) throw new Exception($"Role {roleName} not found");
        return role;
    }

    public record Args(string Name, string RoleName, Input<string> PrincipalId,
        PrincipalType PrincipalType, Input<string>? Scope = default, InputList<Resource>? DependsOn = default);

    public static async Task<RoleAssignment> Create(Args args)
    {
        var scope = args.Scope ?? AzureEnv.DefaultScope;
        var role = await GetRoleDefinitionByName(args.RoleName);

        return new RoleAssignment(
            $"{args.Name}-{args.RoleName.Replace(" ", string.Empty)}",
            new RoleAssignmentArgs
            {
                PrincipalId = args.PrincipalId,
                PrincipalType = args.PrincipalType,
                RoleDefinitionId = role.Id,
                Scope = scope,
            }, new CustomResourceOptions { DependsOn = args.DependsOn ?? new InputList<Resource>() });
    }
}