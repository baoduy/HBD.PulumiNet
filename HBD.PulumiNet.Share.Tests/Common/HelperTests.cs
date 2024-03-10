using FluentAssertions;
using HBD.PulumiNet.Common;
using Xunit;

namespace HBD.PulumiNet.Share.Tests.Common;

public class HelperTests
{
    [Fact]
    public void GetDomainOnly()
    {
        "https://app.drunkcoding.net".GetDomainNameOnly()
            .Should().Be("app.drunkcoding.net");
    }
    
    [Fact]
    public void GetRootDomainOnly()
    {
        "https://app.drunkcoding.net".GetRootDomainOnly()
            .Should().Be("drunkcoding.net");
    }
}