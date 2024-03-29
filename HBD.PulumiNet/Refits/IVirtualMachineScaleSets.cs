using Refit;

namespace HBD.PulumiNet.Refits;

public interface IVirtualMachineScaleSets
{
    [Get("/resourceGroups/{groupName}/providers/Microsoft.Compute/virtualMachineScaleSets?api-version=2020-06-01")]
    Task<AzResult<AzureResourceItem>>GetAsync(string groupName);
}