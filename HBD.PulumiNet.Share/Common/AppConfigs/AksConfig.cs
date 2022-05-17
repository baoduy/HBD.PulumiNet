using HBD.PulumiNet.Share.Types;
// ReSharper disable UnusedMember.Global

namespace HBD.PulumiNet.Share.Common.AppConfigs;

public static class AksConfig
{
    public static string AksGroupName = "aks";
    public static string AksClusterName = Config.OrganizationName;
    public static bool PrivateCluster = false;
    public static bool EnableVirtualNode = false;
    public static bool EnableFirewall = false;

    public static bool EnableCertManager = true;

//Vnet Peering
    public static string? PeeringFirewallIpAddress = null;
    public static string? PeeringVnetId = null;

// Only enabled Dedicated Firewall if no peering Vnet is provided.
// 172.17.0.1/16 is docker bridge and AKS clusters may not use 169.254.0.0/16, 172.30.0.0/16, 172.31.0.0/16, or 192.0.2.0/24 for the Kubernetes service address range, pod address range or cluster virtual network address range.
//- PRD:     192.168.0.1 - 192.168.7.254
//- SANDBOX: 192.167.0.1 - 192.167.7.254
    public static string EnvSpace = AzureEnv.IsPrd ? "192.168.0" : "192.167.0";

//There are 2048 IpAddress for each Environment.
    public static string[] VnetAddressSpace = new[] { $"{EnvSpace}.0/21" };

//There are 254 address for AKS cluster
// Used: 172.xx.1.1 - 172.xx.1.254
    public static string AksSpace = $"{EnvSpace.Replace(".0", ".1")}.0/24";
    public static string AksVirtualNodeSpace = $"{EnvSpace.Replace(".0", ".2")}.0/24";

//There are 126 for Firewall (it needs at least 64 Ip addresses).
// Used: 172.xx.0.1 - 172.xx.0.126
    public static string FirewallSpace = $"{EnvSpace}.0/25";

//This will be use for all Private Links
//Used: 172.xx.0.129 - 172.xx.0.158
    public static string PrivateSpace = $"{EnvSpace}.128/27";

//These IP address must be under aksSpace
    public static string
        InternalAppIp =
            $"{EnvSpace.Replace(".0", ".1")}.250"; //<== all external requests will be DNAT to this IP Address

    public static string
        InternalApiIp =
            $"{EnvSpace.Replace(".0", ".1")}.251"; //<== all API management requests will be DNAT to this IP Address

    public static AzureUserInfo[] DefaultAksAdmins = new[]
    {
        new AzureUserInfo("Steven", "2f65f90a-991c-4f8f-95b3-86e8352240fd"),
        new AzureUserInfo("Steven 2", "188d4296-025a-4042-88f1-c530057efc2d"),
    };
}