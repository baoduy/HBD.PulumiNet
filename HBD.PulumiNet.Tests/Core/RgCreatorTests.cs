using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.PulumiNet.Core;
using Pulumi;
using Pulumi.AzureNative.Resources;
using Xunit;

namespace HBD.PulumiNet.Tests.Core;

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
        var name = group!.GetResourceName();
        name.Should().Be("teststack-group-grp-testorganization");
    }
}