using Pulumi;

namespace HBD.PulumiNet.Share.Common;

public static class StackEnv
{
    public static string ProjectName => Deployment.Instance.ProjectName;
    public static string StackName => Deployment.Instance.StackName;
    public static readonly Pulumi.Config Config = new ();
}