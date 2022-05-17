using Pulumi;
// ReSharper disable NotAccessedField.Global

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace HBD.PulumiNet.Share.Common;

public enum Environments
{
    Global = 1 << 0,
    Dev = 1 << 1,
    Sandbox = 1 << 2,
    Prd = 1 << 3,
}

public static class AzureEnv
{
    public static readonly Output<string> TenantId;
    public static readonly Output<string> SubscriptionId;
    public static readonly Output<string> CurrentServicePrincipal;
    public static readonly string DefaultLocation;
    public static readonly Output<string> DefaultScope;

    /** ======== Default Variables ================*/
    public const bool LockResourceGroup = true;

    public static Dictionary<string, string> DefaultTags = new()
    {
        ["environment"] = StackEnv.StackName,
        ["organization"] = Config.OrganizationName,
        ["pulumi-project"] = StackEnv.ProjectName,
    };

    public static InputList<string> DefaultOwners = new InputList<string>();
    
    /** Enable Rbac or using Access policy for Key Vault*/
    public const bool VaultEnableRbac = true;

    public static readonly bool IsDev;
    public static readonly bool IsSandbox;
    public static readonly bool IsPrd;
    public static readonly Environments CurrentEnv;
    
    /** Protect Private API that only allows accessing from APIM */
    public static bool ApimProtectionEnabled => IsPrd;
    
    public static readonly string EnvDomain = "drunkcoding.net";
        
    static AzureEnv()
    {
        var client = Output.Create(Pulumi.AzureNative.Authorization.GetClientConfig.InvokeAsync());
        TenantId = client.Apply(c => c.TenantId);
        SubscriptionId = client.Apply(c => c.SubscriptionId);
        CurrentServicePrincipal = client.Apply(c => c.ObjectId);
        DefaultScope = SubscriptionId.Apply(s => $"/subscriptions/${s}");
        DefaultLocation = new Pulumi.Config("azure-native").Require("location");

        IsDev = IsEnv(Environments.Dev);
        IsPrd = IsEnv(Environments.Prd);
        IsSandbox = IsEnv(Environments.Sandbox);
        CurrentEnv = GetCurrentEnv();
        
        //Print console
        Output.All(SubscriptionId, TenantId).Apply(s =>
        {
            Console.WriteLine($"Current Azure: TenantId {s[1]}, SubscriptionId {s[0]}");
            return string.Empty;
        });
    }
    
    public static bool IsEnv(Environments env) => StackEnv.StackName.Contains(env.ToString().ToLower());
    
    public static Environments GetCurrentEnv() {
        if (IsPrd) return Environments.Prd;
        if (IsSandbox) return Environments.Sandbox;
        if (IsDev) return Environments.Dev;
        
        return Environments.Global;
    }
}