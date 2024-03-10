using System.Text;
using HBD.PulumiNet.Refits;

namespace HBD.PulumiNet.Common;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class Helpers
{
    /// <summary>
    /// Convert String to Base64
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string ToBase64(this string value)
        => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));

    /// <summary>
    /// Convert Base64 to String
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FromBase64(this string value)
        => Encoding.UTF8.GetString((Convert.FromBase64String(value)));

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