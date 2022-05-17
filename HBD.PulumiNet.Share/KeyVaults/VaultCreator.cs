using HBD.PulumiNet.Share.Common;
using HBD.PulumiNet.Share.Types;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;

// ReSharper disable CollectionNeverUpdated.Global

namespace HBD.PulumiNet.Share.KeyVaults;

public static class VaultCreator
{
    public class VaultNetwork
    {
        public IList<string> IpAddress { get; set; } = new List<string>();
        public IList<string> SubnetIds { get; set; } = new List<string>();
    }

    public record PermissionsArgs(Input<string> PrincipalId, PrincipalType PrincipalType, Permissions.Type Permission,Input<string>? ApplicationId = null);
    
    public record Args(string Name, ResourceGroupInfo Group,
        List<PermissionsArgs> Permissions,
        ConventionArgs? Convention = default,
        bool EnableRbac = false, VaultNetwork? Network = default);

    public static async Task<(Vault vault, AzResourceInfo info)> Create(Args args)
    {
        var name = args.Name.GetKeyVaultName(args.Convention);

        var accessPolicies = new InputList<AccessPolicyEntryArgs>();
        //Grant Access permission
        if (!args.EnableRbac)
        {
            if (args.Permissions.Count <= 0)
                args.Permissions.Add(new PermissionsArgs(AzureEnv.CurrentServicePrincipal, PrincipalType.ServicePrincipal, 
                    Permissions.Type.ReadWrite));

            foreach (var p in args.Permissions)
            {
                accessPolicies.Add(new AccessPolicyEntryArgs
                {
                    Permissions = p.Permission == Permissions.Type.ReadWrite
                        ? Permissions.KeyVaultAdminPolicy
                        : Permissions.KeyVaultReadOnlyPolicy,
                    ApplicationId = p.ApplicationId,
                    ObjectId = p.PrincipalId,
                    TenantId = AzureEnv.TenantId
                });
            }
        }

        var vault = new Vault(name, new VaultArgs
        {
            VaultName = name,
            Location = args.Group.Location,
            ResourceGroupName = args.Group.ResourceGroupName,
            Properties = new VaultPropertiesArgs
            {
                Sku = new SkuArgs {Family = SkuFamily.A, Name = SkuName.Standard}, TenantId = AzureEnv.TenantId,
                CreateMode = CreateMode.Default, EnabledForDeployment = false,
                EnableRbacAuthorization = args.EnableRbac, AccessPolicies = accessPolicies,
                EnablePurgeProtection = true,
                EnableSoftDelete = true,
                EnabledForDiskEncryption = true,
                SoftDeleteRetentionInDays = AzureEnv.IsPrd ? 90 : 7,
                NetworkAcls = args.Network == null
                    ? new NetworkRuleSetArgs
                        {Bypass = NetworkRuleBypassOptions.AzureServices, DefaultAction = NetworkRuleAction.Allow}
                    : new NetworkRuleSetArgs
                    {
                        Bypass = NetworkRuleBypassOptions.AzureServices, DefaultAction = NetworkRuleAction.Deny,
                        IpRules = args.Network.IpAddress.Select(i => new IPRuleArgs {Value = i}).ToList(),
                        VirtualNetworkRules = args.Network.SubnetIds.Select(s => new VirtualNetworkRuleArgs {Id = s})
                            .ToList()
                    }
            },
            Tags = AzureEnv.DefaultTags,
        });

        //Grant RBAC permission
        if (args.EnableRbac) {
            foreach (var p in args.Permissions)
                await Permissions.GrantVaultRbacPermission(new Permissions.Args(name, p.PrincipalId, p.PrincipalType,
                    p.Permission, vault));
        }

        var info = new AzResourceInfo(name, args.Group, vault.Id);
        return (vault,info);
    }

    public static Vault EnableDiagnostic(this Vault vault, Core.Helpers.LogDestinationArgs destination) 
        => Core.Helpers.EnableDiagnostic(vault, destination, logs: new[] {"AuditEvent"});
    
    //public static Vault EnablePrivateLink(this Vault vault,)
}