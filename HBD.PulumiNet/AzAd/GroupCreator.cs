using Pulumi.AzureAD;
using Pulumi.AzureNative.Authorization;

namespace HBD.PulumiNet.AzAd;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public class GroupCreator
{
    public record GroupPermissionArgs(string RoleName, Input<string>? Scope = null);

    public record Args(string Name)
    {
        public InputList<string>? Owners { get; init; }
        public Input<string>[]? Members { get; init; }
        public GroupPermissionArgs[]? Permissions { get; init; }
    }

    public static Group Create(Args args)
    {
        var name = args.Name.ToUpper();

        var group = new Group(name,
            new GroupArgs
            {
                DisplayName = name,
                Owners = args.Owners!,
                MailEnabled = false,
                SecurityEnabled = true,
                ExternalSendersAllowed = false
            });

        if (args.Members != null)
        {
            var count = 1;
            foreach (var m in args.Members)
            {
                _ = new GroupMember($"{args.Name}-member-{count++}",
                    new GroupMemberArgs { GroupObjectId = group.ObjectId, MemberObjectId = m });
            }
        }

        if (args.Permissions != null)
        {
            foreach (var p in args.Permissions)
            {
                RoleAssignments.Assign(new RoleAssignments.Args(args.Name, p.RoleName, group.ObjectId,
                    PrincipalType.Group, Scope: p.Scope, DependsOn: group));
            }
        }

        return group;
    }

    public static Output<GetGroupResult> Get(string name) =>
        GetGroup.Invoke(new GetGroupInvokeArgs { DisplayName = name });
}