using Pulumi.AzureNative.Resources;

namespace HBD.PulumiNet.Core;

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