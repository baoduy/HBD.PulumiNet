using Pulumi;
using Pulumi.AzureAD;
using Pulumi.AzureNative.Authorization;

namespace HBD.PulumiNet.Share.AzAd;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public class GroupCreator
{
    public record GroupPermissionArgs(string RoleName, Input<string>? Scope = null);

    public record Args(string Name,
        InputList<string> Owners = null,
        Input<string>[]? Members = default,
        GroupPermissionArgs[]? Permissions = null);

    public static async Task<Group> Create(Args args)
    {
        var group = new Group(args.Name,
            new GroupArgs { DisplayName = args.Name.ToUpper(), Owners = args.Owners });

        if (args.Members != null)
        {
            var count = 1;
            foreach (var m in args.Members)
            {
                var _ = new GroupMember($"{args.Name}-member-{count++}",
                    new GroupMemberArgs { GroupObjectId = group.ObjectId, MemberObjectId = m });
            }
        }

        if (args.Permissions != null)
        {
            foreach (var p in args.Permissions)
            {
                await RoleAssignments.Create(new RoleAssignments.Args(args.Name, p.RoleName, group.ObjectId,
                    PrincipalType.Group, Scope: p.Scope, DependsOn: group));
            }
        }

        return group;
    }
}