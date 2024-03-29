

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace HBD.PulumiNet.Types;

public record BasicArgs(string Name,ResourceGroupInfo Group)
{
    public bool Lock { get; set; }
}

public record ConventionArgs(string? Prefix = null, string? Suffix = null);

public record ResourceGroupInfo
{
    public ResourceGroupInfo(string resourceGroupName, Input<string>? location = null)
    {
        ResourceGroupName = resourceGroupName;
        Location = location ?? AzureEnv.CurrentLocation;
    }

    public string ResourceGroupName { get; }
    public Input<string> Location { get; }
}

public record AzResourceInfo(string Name, ResourceGroupInfo Group, Output<string> Id);

public class AzResult<TResult> where TResult : class
{
    public IList<TResult> Value { get; set; } = new List<TResult>();
}

public class AzureResourceItem
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string ResourceGroupName { get; internal set; } = null!;

    public Dictionary<string, string> Tags { get; set; } = new();
}

public record ResourceInfoResult(string Name, ResourceGroupInfo Group, Output<string> Id);

public class ResourceInfoArgs
{
    /**If name and provider of the resource is not provided then the Id will be resource group Id*/
    public Input<string>? Name { get; set; }

    /**The provider name of the resource ex: "Microsoft.Network/virtualNetworks" or "Microsoft.Network/networkSecurityGroups"*/
    public string? Provider { get; set; }

    public ResourceGroupInfo Group { get; set; } = null!;

    public Output<string> SubscriptionId { get; set; } = AzureEnv.SubscriptionId;
}

public record BasicMonitorArgs(Output<string>? LogWpInfo = null, Output<string>? LogStorageId = null);

public record AzureUserInfo(string Name, string ObjectId);