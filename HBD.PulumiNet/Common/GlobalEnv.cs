namespace HBD.PulumiNet.Common;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class GlobalEnv
{
    public const string GlobalKeyName = "global";

    public static readonly ConventionArgs GlobalConvention = new(GlobalKeyName, $"grp-{StackEnv.OrganizationName}");

    public static readonly ResourceGroupInfo GroupInfo = new(GlobalKeyName.AsResourceName(GlobalConvention));

    public static readonly ResourceGroupInfo LogGroupInfo = new("logs".AsResourceName(GlobalConvention));

    public static readonly AzResourceInfo CdnProfileInfo = new(
        Name: $"{GlobalKeyName}-{StackEnv.OrganizationName}".GetCdnProfileName(),
        Group: GroupInfo,
        Id: Output.Format(
            $"/subscriptions/{AzureEnv.SubscriptionId}/resourceGroups/{GroupInfo.ResourceGroupName}/providers/Microsoft.Cdn/profiles/{GlobalKeyName}-{StackEnv.OrganizationName}-cdn-pfl"));

    public static readonly AzResourceInfo KeyVaultInfo = new(
        Name: $"{GlobalKeyName}-{StackEnv.OrganizationName}".GetKeyVaultName(new ConventionArgs(Prefix: "")),
        Group: GroupInfo,
        Id: Output.Format(
            $"/subscriptions/{AzureEnv.SubscriptionId}/resourceGroups/{GroupInfo.ResourceGroupName}/providers/Microsoft.KeyVault/vaults/{GlobalKeyName}-{StackEnv.OrganizationName}-vlt"));

    public static readonly BasicMonitorArgs LogWpInfo = new(LogWpInfo: Output.Format(
        $"/subscriptions/{AzureEnv.SubscriptionId}/resourceGroups/{LogGroupInfo.ResourceGroupName}/providers/Microsoft.Web/sites/{GlobalKeyName}-{StackEnv.OrganizationName}-log"));

    public static readonly BasicMonitorArgs LogStorageInfo = new(LogStorageId: Output.Format(
        $"/subscriptions/{AzureEnv.SubscriptionId}/resourceGroups/{LogGroupInfo.ResourceGroupName}/providers/Microsoft.Storage/storageAccounts/{GlobalKeyName}{StackEnv.OrganizationName}logsstg"));
}