using HBD.PulumiNet.Share.Types;

// ReSharper disable MemberCanBePrivate.Global

namespace HBD.PulumiNet.Share.Common;

public enum ConnectionType
{
    Primary = 1,
    Secondary = 2,
}

/// <summary>
/// Synced on 24/Jan/2023
/// </summary>
public static class Naming
{
    /// <summary>
    ///  The method to get Resource group Name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetResourceGroupName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: string.IsNullOrEmpty(StackEnv.OrganizationName)
            ? "grp"
            : $"grp-{StackEnv.OrganizationName}"));

    /// <summary>
    /// Get Azure Storage Account and CosmosDb Name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static string GetStorageName(this string name)
    {
        name = name.AsResourceName(new ConventionArgs(Suffix: "stg"));
        name = name.Replace("-", string.Empty)
            .Replace(".", string.Empty);

        return name.ToLower()[..24];
    }

    /** Get Vault Secret Name. Remove the stack name and replace all _ with - then lower cases. */
    public static string GetSecretName(this string name) =>
        name.Replace($"{StackEnv.StackName}-", string.Empty)
            .Replace(StackEnv.StackName, string.Empty)
            .Replace(" ", "-")
            .Replace("_", "-")
            .ToLower();

    public static string GetCertName(this string name)
    {
        var n = GetSecretName(name);
        return $"{n}-cert";
    }

    public static string GetConnectionName(this string name, ConnectionType type) =>
        $"{GetSecretName(name)}-conn-{type}".ToLower();

    public static string GetKeyName(this string name, ConnectionType type) =>
        $"{GetSecretName(name)}-key-{type}".ToLower();

    public static string GetPasswordName(this string name, ConnectionType? type = null) =>
        type == null
            ? name.AsResourceName(new ConventionArgs(Suffix: "pwd"))
            : $"{GetSecretName(name)}-pwd-{type}".ToLower();

    public static string GetAutomationAccountName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "acc-auto"));

    public static string GetB2cName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "b2c"));
    
    public static string GetCosmosDbName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "cdb"));

    public static string GetAppConfigName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "cfg"));

    public static string GetApimName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "apim"));

    public static string GetSshName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "ssh"));
    
    public static string GetIdentityName(this string name) =>
        name.AsResourceName();

    public static string GetAksName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "aks"));

    public static string GetK8sProviderName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "ks-pvd"));

    public static string GetAppInsightName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "isg"));

    public static string GetLogWpName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "log"));
    
    public static string GetWebTestName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "hlz"));

    public static string GetAlertName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "alt"));
    
    public static string GetRedisCacheName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "rds"));
    
    public static string GetServiceBusName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "bus"));

    public static string GetPrivateEndpointName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "pre"));
    
    public static string GetSignalRName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "sigr"));
    
    public static string GetElasticPoolName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "elp"));
    
    public static string GetSqlDbName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "db"));
    
    public static string GetSqlServerName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "sql"));

    public static string GetFirewallName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "fw"));
    
    public static string GetFirewallPolicyName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "fwp"));

    public static string GetFirewallPolicyGroupName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "fw-pg"));

    public static string GetVmName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "vm"));

    public static string GetNicName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "nic"));

    public static string GetVnetName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "vnt"));
    
    public static string GetWanName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "wan"));
    
    public static string GetHubName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "hub"));
    
    public static string GetRouteName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "route"));

    public static string GetRouteItemName(this string name) =>
        name.AsResourceName(new ConventionArgs{Suffix = string.Empty});
    
    public static string GetNetworkSecurityGroupName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "nsg"));
    
    public static string GetIpAddressName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "ip"));
    
    public static string GetIpAddressPrefixName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "ipx"));
    
    public static string GetAppGatewayName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "gwt"));

    public static string GetBastionName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "bst"));

    public static string GetKeyVaultName(this string name, ConventionArgs? convention = default)
    {
        var n = name.AsResourceName(convention == null
            ? new ConventionArgs(Suffix: "vlt")
            : convention.MergeWith(new ConventionArgs(Suffix: "vlt")));
        
        return n.Length > 24 ? n[..24] : n;
    }

    public static string GetCdnEndpointName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "cdn"));

    public static string GetCdnProfileName(this string name) =>
        name.AsResourceName(new ConventionArgs(Suffix: "cdn-pfl"));

    public static string GetAcrName(this string name) =>
        name.AsResourceName(new ConventionArgs(Prefix: "", Suffix: "acr")).Replace("-", "4");

    public static string GetCertOrderName(this string name) =>
        name.Replace(".", "-").AsResourceName(new ConventionArgs(Prefix: "", Suffix: "ca"));
}