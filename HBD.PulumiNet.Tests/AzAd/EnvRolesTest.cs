using FluentAssertions;
using HBD.PulumiNet.AzAd;
using HBD.PulumiNet.Common;
using Xunit;

namespace HBD.PulumiNet.Tests.AzAd;

public class EnvRolesTest
{
    [Fact]
    public void GetEnvRolesTest()
    {
        var roles = EnvRoles.GetEnvRoles(Environments.Dev);
        roles.ReadOnly.Should().Be("ROL NON-PRD AZURE READONLY");
        roles.Admin.Should().Be("ROL NON-PRD AZURE ADMIN");
        roles.Contributor.Should().Be("ROL NON-PRD AZURE CONTRIBUTOR");
    }
}