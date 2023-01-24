using System.Text;
using HBD.PulumiNet.Share.Types;

namespace HBD.PulumiNet.Share.Common;

public static class ResourceEnv
{
    public static readonly ConventionArgs ResourceConvention = new(StackEnv.StackName);

    /// <summary>
    /// Format Name with convention.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="convention"></param>
    /// <returns></returns>
    public static string FormatWith(this string name, ConventionArgs convention)
    {
        if (string.IsNullOrWhiteSpace(name)) return name;

        var builder = new StringBuilder();
        
        var (prefix, suffix) = convention;

        //Add prefix
        if (!string.IsNullOrEmpty(prefix) && !name.StartsWith(prefix))
            builder.Append(prefix).Append('-');

        //Add Name
        builder.Append(name).Replace(" ", "-");
        
        //Add the suffix
        if (!string.IsNullOrEmpty(suffix) && !name.EndsWith(suffix))
            builder.Append('-').Append(suffix);

        return builder.ToString().ToLower();
    }

    /// <summary>
    /// The method to get Resource Name. This is not applicable for Azure Storage Account and CosmosDb
    /// </summary>
    /// <returns></returns>
    public static string AsResourceName(this string name, ConventionArgs? convention = default)
        => name.FormatWith(convention == null
            ? ResourceConvention
            : convention.MergeWith(ResourceConvention));
}