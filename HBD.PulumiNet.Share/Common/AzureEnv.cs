// ReSharper disable NotAccessedField.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

using System.Text.Json;

namespace HBD.PulumiNet.Share.Common;

public enum Environments
{
    Global = 1 << 0,
    Dev = 1 << 1,
    Sandbox = 1 << 2,
    Prd = 1 << 3,
}

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class AzureEnv
{
    public static readonly Output<string> TenantId;
    public static readonly Output<string> SubscriptionId;
    public static readonly Output<string> CurrentPrincipal;
    public static readonly string CurrentLocation;
    public static readonly Output<string> DefaultScope;

    public static readonly Dictionary<string, string> DefaultTags = new()
    {
        ["environment"] = StackEnv.StackName,
        ["organization"] = StackEnv.OrganizationName,
        ["pulumi-project"] = StackEnv.ProjectName,
    };

    public static readonly bool IsDev;
    public static readonly bool IsSandbox;
    public static readonly bool IsPrd;
    public static readonly Environments CurrentEnv;

    static AzureEnv()
    {
        var config = Pulumi.AzureNative.Authorization.GetClientConfig.Invoke();

        TenantId = config.Apply(c => c.TenantId);
        SubscriptionId = config.Apply(c => c.SubscriptionId);
        CurrentPrincipal = config.Apply(c => c.ObjectId);
        DefaultScope = SubscriptionId.Apply(s => $"/subscriptions/{s}");
        CurrentLocation =
            JsonSerializer.Deserialize<Dictionary<string, string>>(Environment.GetEnvironmentVariable("PULUMI_CONFIG")!)
                !["azure-native:location"];

        IsDev = IsEnv(Environments.Dev);
        IsPrd = IsEnv(Environments.Prd);
        IsSandbox = IsEnv(Environments.Sandbox);
        CurrentEnv = GetCurrentEnv();

        //Print console
        Output.Format(
                $"\n\tTenantId {TenantId}, \n\tSubscriptionId {SubscriptionId}, \n\tLocation: {CurrentLocation}")
            .Apply(s =>
            {
                Console.WriteLine("Azure Environment: {0}", s);
                return string.Empty;
            });
    }

    public static bool IsEnv(Environments env) => StackEnv.StackName.Contains(env.ToString().ToLower());

    public static Environments GetCurrentEnv()
    {
        if (IsPrd) return Environments.Prd;
        if (IsSandbox) return Environments.Sandbox;
        return IsDev ? Environments.Dev : Environments.Global;
    }

    /// <summary>
    /// Get Key Vault Info
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public static ResourceInfoResult GetKeyVaultInfo(string groupName)
    {
        var vaultName = groupName.GetKeyVaultName();
        var resourceGroupName = groupName.GetResourceGroupName();

        return new ResourceInfoResult(
            vaultName,
            new ResourceGroupInfo(resourceGroupName),
            Output.Format(
                $"/subscriptions/{SubscriptionId}/resourceGroups/{resourceGroupName}/providers/Microsoft.KeyVault/vaults/{vaultName}"));
    }

    /// <summary>
    /// Get resource information from resource id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static ResourceInfoResult? GetResourceInfoFromId(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;

        var details = id.Split("/");
        var name = "";
        var groupName = "";
        var subscriptionId = "";

        for (var i = 0; i < details.Length; i++)
        {
            var d = details[i].ToLower();

            switch (d)
            {
                case "subscriptions":
                    subscriptionId = details[i + 1];
                    break;
                case "resourcegroups":
                    groupName = details[i + 1];
                    break;
            }

            name = d; //Name is the last item in the array.
        }


        return new ResourceInfoResult(name, new ResourceGroupInfo(groupName), Output.Create(id));
    }

    /// <summary>
    /// Build Resource Id from Information
    /// </summary>
    /// <param name="props"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Output<string> GetResourceIdFromInfo(ResourceInfoArgs props)
    {
        if (props.Name == null && string.IsNullOrEmpty(props.Provider))
            return Output.Format(
                $"/subscriptions/{props.SubscriptionId}/resourceGroups/{props.Group.ResourceGroupName}");

        if (props.Name != null && !string.IsNullOrEmpty(props.Provider))
            return Output.Format(
                $"/subscriptions/{props.SubscriptionId}/resourceGroups/{props.Group.ResourceGroupName}/providers/{props.Provider}/{props.Name}");

        throw new ArgumentException("Resource Info is invalid.");
    }
}