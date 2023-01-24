using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.PulumiNet.Share.AzAd;
using HBD.PulumiNet.Share.Core;
using HBD.PulumiNet.Share.Tests.Stacks;
using Pulumi;
using Pulumi.AzureAD;
using Pulumi.AzureNative.Resources;
using Xunit;

namespace HBD.PulumiNet.Share.Tests.AzAd;

public class RgCreatorTests
{
    internal class RgCreatorTestStack : Stack
    {
        public RgCreatorTestStack()
        {
            var group = Output.Create( RgCreator.Create(new RgCreator.Args("group")));
        }
    }
    
    [Fact]
    public async Task RgCreatorTest()
    {
        var rs = await Testing.RunAsync<RgCreatorTestStack>();
      
        var group = rs.OfType<ResourceGroup>().FirstOrDefault();
        var name = group.GetResourceName();
        name.Should().Be("teststack-group-grp-testorganization");
    }
}