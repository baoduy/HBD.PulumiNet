using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HBD.PulumiNet.AzAd;
using HBD.PulumiNet.Common;
using Pulumi;
using Pulumi.AzureAD;
using Xunit;

namespace HBD.PulumiNet.Tests.AzAd;

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