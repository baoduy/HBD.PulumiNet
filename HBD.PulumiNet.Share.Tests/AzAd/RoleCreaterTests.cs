using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.PulumiNet.Share.AzAd;
using HBD.PulumiNet.Share.Common;
using HBD.PulumiNet.Share.Tests.Stacks;
using Pulumi;
using Pulumi.AzureAD;
using Xunit;

namespace HBD.PulumiNet.Share.Tests.AzAd;

public class RoleCreatorTests
{
    private class AdGroupStack : Stack
    {
        public AdGroupStack()
        {
            var rs = Output.Create(RoleCreator.Create(
                new RoleCreator.Args(Environments.Dev, "TEST", "ST")));
        }
    }

    [Fact]
    public async Task RoleCreatorTest()
    {
        var rs = await Testing.RunAsync<AdGroupStack>();
        rs.Length.Should().BeGreaterOrEqualTo(1);
        var group = rs.OfType<Group>().FirstOrDefault();
        group.Should().NotBeNull();

        var name = await group.DisplayName.GetValueAsync();
        name.Should().Be("ROL NON-PRD GLB TEST ST");
    }
}