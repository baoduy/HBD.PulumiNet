using HBD.PulumiNet.Share.Types;
using Pulumi;

namespace HBD.PulumiNet.Share.Common;

public static class ResourceEnv
{
    /// <summary>
    /// Format Name with convention.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="convention"></param>
    /// <returns></returns>
    public static string FormatWith(this string name, ConventionArgs convention)
    {
        if (string.IsNullOrWhiteSpace(name)) return name;
        name = name.Replace(" ", "-");

        //Add prefix
        var (prefix, suffix) = convention;

        if (!string.IsNullOrEmpty(prefix) && !name.StartsWith(prefix))
            name = prefix + "-" + name;

        //Add the suffix
        if (!string.IsNullOrEmpty(suffix) && !name.EndsWith(suffix))
            name = name + "-" + suffix;

        return name.ToLower();
    }

    /// <summary>
    /// The method to get Resource Name. This is not applicable for Azure Storage Account and CosmosDb
    /// </summary>
    /// <returns></returns>
    public static string AsResourceName(this string name, ConventionArgs? convention = default)
        => name.FormatWith(convention == null
            ? Config.ResourceConvention
            : convention.MergeWith(Config.ResourceConvention));

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


        return new ResourceInfoResult(name, new ResourceGroupInfo(groupName), subscriptionId, id);
    }
    
    /// <summary>
    /// Build Resource Id from Information
    /// </summary>
    /// <param name="props"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Output<string> GetResourceIdFromInfo (ResourceInfoArgs props) {
        if (props.Name==null && string.IsNullOrEmpty(props.Provider))
            return Output.Format($"/subscriptions/${props.SubscriptionId}/resourceGroups/${props.Group.ResourceGroupName}");
        else if (props.Name!=null && !string.IsNullOrEmpty(props.Provider))
            return Output.Format($"/subscriptions/${props.SubscriptionId}/resourceGroups/${props.Group.ResourceGroupName}/providers/${props.Provider}/${props.Name}");

        throw new ArgumentException("Resource Info is invalid.");
    }
}