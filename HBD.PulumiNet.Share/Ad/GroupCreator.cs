using HBD.PulumiNet.Share.Common;
using Pulumi;
using Pulumi.AzureAD;
using Pulumi.AzureNative.Authorization;

namespace HBD.PulumiNet.Share.Ad;

public class GroupCreator
{
    public record GroupPermissionArgs(string RoleName, Input<string>? Scope = null);

    public record Args(string Name, Input<string>[]? Members = default,
        GroupPermissionArgs[]? Permissions = null);

    public static async Task<Group> Create(Args args)
    {
        var group = new Group(args.Name,
            new GroupArgs { DisplayName = args.Name, Owners = AzureEnv.DefaultOwners });

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