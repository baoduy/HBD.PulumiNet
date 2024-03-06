using Refit;

namespace HBD.PulumiNet.Share.Refits;

public interface INetworkSecurityGroup
{
    [Get("/resourceGroups/{groupName}/providers/Microsoft.Network/networkSecurityGroups?api-version=2020-05-01")]
    Task<AzResult<AzureResourceItem>>GetAsync(string groupName);
}