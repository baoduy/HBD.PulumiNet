using FluentAssertions;
using HBD.PulumiNet.Common;
using HBD.PulumiNet.Types;
using Xunit;

namespace HBD.PulumiNet.Tests.Common;

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