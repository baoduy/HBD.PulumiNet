using FluentAssertions;
using HBD.PulumiNet.AzAd;
using Xunit;

namespace HBD.PulumiNet.Tests.AzAd;

public class RolesBuiltInTests
{
    [Fact]
    public void FindRolesTest()
    {
        RolesBuiltIn.Find("Reader").Should().BeOfType<RolesBuiltIn.AzRole>();
        RolesBuiltIn.Find("Contributor").Should().BeOfType<RolesBuiltIn.AzRole>();
        RolesBuiltIn.Find("Advisor Reviews Contributor").Should().BeOfType<RolesBuiltIn.AzRole>();
        RolesBuiltIn.Find("Azure AI Developer").Should().BeOfType<RolesBuiltIn.AzRole>();
        RolesBuiltIn.Find("Azure Arc Kubernetes Cluster Admin").Should().BeOfType<RolesBuiltIn.AzRole>();
    }
}