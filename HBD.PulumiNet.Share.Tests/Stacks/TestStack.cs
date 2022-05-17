using Pulumi;
using Pulumi.AzureNative.Resources;

namespace HBD.PulumiNet.Share.Tests.Stacks;

public class TestStack : Stack
{
    public TestStack()
    {
        var client = Output.Create(Pulumi.AzureNative.Authorization.GetClientConfig.InvokeAsync());
        client.Apply(c =>
        {
            return c.SubscriptionId;
        });
        
        var group = new ResourceGroup("AA", new ResourceGroupArgs());
    }
}