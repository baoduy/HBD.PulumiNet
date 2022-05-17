using HBD.PulumiNet.Share.Types;
using Pulumi;

namespace HBD.PulumiNet.Share.Common;

public static class GlobalEnv
{
    public static readonly ResourceGroupInfo GroupInfo =
        new(Config.GlobalKeyName.AsResourceName(Config.GlobalConvention));

    public static readonly ResourceGroupInfo LogGroupInfo = new("log".AsResourceName(Config.GlobalConvention));

    public static readonly AzResourceInfo KeyVaultInfo = new AzResourceInfo(
        Name: $"{Config.GlobalKeyName}-{Config.OrganizationName}".GetKeyVaultName(new ConventionArgs(Prefix: "")),
        Group: GroupInfo,
        Id: Output.Format(
            $"/subscriptions/${AzureEnv.SubscriptionId}/resourceGroups/${GroupInfo.ResourceGroupName}/providers/Microsoft.KeyVault/vaults/${Config.GlobalKeyName}-${Config.OrganizationName}-vlt"));

    public static readonly BasicMonitorArgs LogWpInfo = new(LogWpInfo: Output.Format(
        $"/subscriptions/${AzureEnv.SubscriptionId}/resourceGroups/${LogGroupInfo.ResourceGroupName}/providers/Microsoft.Web/sites/${Config.GlobalKeyName}-${Config.OrganizationName}-log"));
    
    public static readonly BasicMonitorArgs LogStorageInfo = new(LogStorageId: Output.Format(
        $"/subscriptions/${AzureEnv.SubscriptionId}/resourceGroups/${LogGroupInfo.ResourceGroupName}/providers/Microsoft.Storage/storageAccounts/${Config.GlobalKeyName}${Config.OrganizationName}logsstg"));
}