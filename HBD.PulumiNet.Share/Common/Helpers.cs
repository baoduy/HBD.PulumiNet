using HBD.PulumiNet.Share.Refits;
using HBD.PulumiNet.Share.Types;
using Pulumi;

namespace HBD.PulumiNet.Share.Common;

public static class Helpers
{
    /// <summary>
    /// Get Domain or Sub-Domain name from url. ex https://app.drunkcoding.net => app.drunkcoding.net
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetDomainNameOnly(this string url) =>
        url.Replace("https://", "").Replace("http://", "").Split("/")[0];

    /// <summary>
    /// Get Root domain from url. ex https://app.drunkcoding.net => drunkcoding.net
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetRootDomainOnly(this string url)
    {
        var array = url.GetDomainNameOnly().Split(".");
        return string.Join(".", array[1..]);
    }

    /// <summary>
    /// Find Network Security Group in Resource Group.
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<AzureResourceItem>> FindNetworkSecurityGroups(string group)
    {
        try
        {
            var client = await Factory.Create<INetworkSecurityGroup>();
            var rs = await client.GetAsync(group);

            return rs.Value.Select(i =>
            {
                i.ResourceGroupName = group;
                return i;
            });
        }
        catch (Exception ex)
        {
            await Console.Error.WriteAsync("FindNetworkSecurityGroups: " + ex.Message);
            return Enumerable.Empty<AzureResourceItem>();
        }
    }

    public static async Task<IEnumerable<AzureResourceItem>> FindVmScaleSets(string group)
    {
        try
        {
            var client = await Factory.Create<IVirtualMachineScaleSets>();
            var rs = await client.GetAsync(group);

            return rs.Value.Select(i =>
            {
                i.ResourceGroupName = group;
                return i;
            });
        }
        catch (Exception ex)
        {
            await Console.Error.WriteAsync("FindVmScaleSets: " + ex.Message);
            return Enumerable.Empty<AzureResourceItem>();
        }
    }

    public static Output<T> AsOutput<T>(this Task<T> task) => Output.Create(task);
}