using HBD.PulumiNet.AzAd;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;
using Core_Helpers = HBD.PulumiNet.Core.Helpers;

// ReSharper disable CollectionNeverUpdated.Global

namespace HBD.PulumiNet.KeyVaults;

public static class VaultCreator
{
    public sealed class VaultNetwork
    {
        public IList<string> IpAddress { get; set; } = new List<string>();
        public IList<string> SubnetIds { get; set; } = new List<string>();
    }

    public record Args(
        string Name,
        ResourceGroupInfo Group,
        EnvRoles.EnvRoleResults? Auth = default,
        VaultNetwork? Network = default);

    public static (Vault vault, AzResourceInfo info) Create(Args args)
    {
        //Always enable RBAC
        const bool enableRbac = true;
        var name = args.Name.GetKeyVaultName();

        var accessPolicies = new InputList<AccessPolicyEntryArgs>();
        //Grant Access permission
        // if (!args.EnableRbac)
        // {
        //     // if (args.Permissions.Count <= 0)
        //     //     args.Permissions.Add(new PermissionsArgs(AzureEnv.CurrentServicePrincipal, PrincipalType.ServicePrincipal,
        //     //         Permissions.Type.ReadWrite));
        //
        //     foreach (var p in args.Permissions)
        //     {
        //         accessPolicies.Add(new AccessPolicyEntryArgs
        //         {
        //             Permissions = p.Permission == Permissions.Type.ReadWrite
        //                 ? Permissions.KeyVaultAdminPolicy
        //                 : Permissions.KeyVaultReadOnlyPolicy,
        //             ApplicationId = p.ApplicationId,
        //             ObjectId = p.PrincipalId,
        //             TenantId = AzureEnv.TenantId
        //         });
        //     }
        // }

        var vault = new Vault(name, new VaultArgs
        {
            VaultName = name,
            Location = args.Group.Location,
            ResourceGroupName = args.Group.ResourceGroupName,
            Properties = new VaultPropertiesArgs
            {
                Sku = new SkuArgs { Family = SkuFamily.A, Name = SkuName.Standard },
                TenantId = AzureEnv.TenantId,
                CreateMode = CreateMode.Default,
                EnabledForDeployment = false,
                EnableRbacAuthorization = enableRbac,
                AccessPolicies = accessPolicies,
                EnablePurgeProtection = true,
                EnableSoftDelete = true,
                EnabledForDiskEncryption = true,
                SoftDeleteRetentionInDays = AzureEnv.IsPrd ? 90 : 7,
                NetworkAcls = args.Network == null
                    ? new NetworkRuleSetArgs
                        { Bypass = NetworkRuleBypassOptions.AzureServices, DefaultAction = NetworkRuleAction.Allow }
                    : new NetworkRuleSetArgs
                    {
                        Bypass = NetworkRuleBypassOptions.AzureServices, DefaultAction = NetworkRuleAction.Deny,
                        IpRules = args.Network.IpAddress.Select(i => new IPRuleArgs { Value = i }).ToList(),
                        VirtualNetworkRules = args.Network.SubnetIds.Select(s => new VirtualNetworkRuleArgs { Id = s })
                            .ToList()
                    }
            },
            Tags = AzureEnv.DefaultTags,
        });

        if (args.Auth is not null)
        {
            var adminGroupId = GroupCreator.Get(args.Auth.Contributor).Apply(g => g.ObjectId);
            var readOnlyGroupId = GroupCreator.Get(args.Auth.ReadOnly).Apply(g => g.ObjectId);

            //TODO: Add current principal into Admin group
           
            RoleAssignments.AssignToGroup(
                new RoleAssignments.GroupRoleArgs(Name: name + "_ReadOnly",
                    GroupId: readOnlyGroupId,
                    Scope: vault.Id),
                "Key Vault Crypto Service Encryption User",
                "Key Vault Crypto User",
                "Key Vault Secrets User");
            RoleAssignments.AssignToGroup(new RoleAssignments.GroupRoleArgs(Name: name + "_Admin",
                    GroupId: adminGroupId,
                    Scope: vault.Id),
                "Key Vault Administrator",
                "Key Vault Crypto User",
                "Key Vault Certificates Officer",
                "Key Vault Crypto Officer",
                "Key Vault Secrets Officer");
        }
        //Grant RBAC permission
        // if (enableRbac)
        // {
        //     foreach (var p in args.Permissions) 
        //         Permissions.GrantVaultRbacPermission(new Permissions.Args(name, p.PrincipalId, p.PrincipalType,
        //             p.Permission, vault));
        // }

        var info = new AzResourceInfo(name, args.Group, vault.Id);
        return (vault, info);
    }

    public static Vault EnableDiagnostic(this Vault vault, Core_Helpers.LogDestinationArgs destination)
        => Core_Helpers.EnableDiagnostic(vault, destination, logs: new[] { "AuditEvent" });

    //public static Vault EnablePrivateLink(this Vault vault,)
}