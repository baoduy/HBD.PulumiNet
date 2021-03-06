using HBD.PulumiNet.Share.Common;
using HBD.PulumiNet.Share.Types;
using Pulumi.AzureNative.Resources;

namespace HBD.PulumiNet.Share.Core;

public static class RgCreator
{
    public record Args(string Name, bool FormattedName = false);

    public static ResourceGroup Create(Args args)
    {
        var name = args.FormattedName ? args.Name : args.Name.GetResourceGroupName();
       return new ResourceGroup(name, new ResourceGroupArgs {ResourceGroupName = name});
    }

    public static ResourceGroupInfo Info(this ResourceGroup group) =>
        new (group.GetResourceName(), group.Location);
}