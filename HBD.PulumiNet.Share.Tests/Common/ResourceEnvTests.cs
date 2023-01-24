using FluentAssertions;
using HBD.PulumiNet.Share.Common;
using HBD.PulumiNet.Share.Types;
using Xunit;

namespace HBD.PulumiNet.Share.Tests.Common;

public class ResourceEnvTests
{
    [Fact]
    public void FormatWithTest()
    {
        const string expected = "hbd-steven-group";
        
        var rs = "steven".FormatWith(new ConventionArgs("hbd", "group"));
        rs.Should().Be(expected);
        
        rs = "steven-group".FormatWith(new ConventionArgs("hbd", "group"));
        rs.Should().Be(expected);
        
        rs = "hbd-steven".FormatWith(new ConventionArgs("hbd", "group"));
        rs.Should().Be(expected);
        
        rs = "hbd-steven-group".FormatWith(new ConventionArgs("hbd", "group"));
        rs.Should().Be(expected);
    }
}