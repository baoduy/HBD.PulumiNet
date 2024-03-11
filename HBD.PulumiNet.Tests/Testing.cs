using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Moq;
using Pulumi;
using Pulumi.Testing;

namespace HBD.PulumiNet.Tests;

public static class Testing
{
    public static Task<ImmutableArray<Resource>> RunAsync<T>() where T : Stack, new()
    {
        var mocks = new Mock<IMocks>();

        mocks.Setup(m => m.NewResourceAsync(It.IsAny<MockResourceArgs>()))
            .ReturnsAsync((MockResourceArgs args) => (args.Id ?? "", args.Inputs));
        
        mocks.Setup(m => m.CallAsync(It.IsAny<MockCallArgs>()))
            .ReturnsAsync((MockCallArgs args) => args.Args);

        Environment.SetEnvironmentVariable("PULUMI_DRY_RUN","false");
        Environment.SetEnvironmentVariable("PULUMI_ORGANIZATION","TestOrganization");
        Environment.SetEnvironmentVariable("PULUMI_PROJECT","TestProject");
        Environment.SetEnvironmentVariable("PULUMI_STACK","TestStack");
        
        return Deployment.TestAsync<T>(mocks.Object, new TestOptions
        {
            IsPreview = false,
            ProjectName = "TestProject", 
            StackName = "TestStack",
            OrganizationName = "TestOrganization"
        });
    }
}