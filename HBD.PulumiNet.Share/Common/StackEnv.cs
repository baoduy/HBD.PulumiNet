using Pulumi;

namespace HBD.PulumiNet.Share.Common;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class StackEnv
{
    public static string OrganizationName => Deployment.Instance.OrganizationName.ToLower();
    public static string ProjectName => Deployment.Instance.ProjectName.ToLower();
    public static string StackName => Deployment.Instance.StackName.ToLower();
}