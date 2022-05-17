using System.Threading.Tasks;
using FluentAssertions;
using HBD.PulumiNet.Share.Tests.Stacks;
using Xunit;

namespace HBD.PulumiNet.Share.Tests.Common;

public class StackTests
{
    [Fact]
    public async Task StackEnv_Tests()
    {
        var resources = await Testing.RunAsync<TestStack>();
        resources.Length.Should().BeGreaterOrEqualTo(1);
        
    }
}