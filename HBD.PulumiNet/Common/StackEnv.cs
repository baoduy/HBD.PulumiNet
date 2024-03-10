namespace HBD.PulumiNet.Common;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class StackEnv
{
    public static readonly bool IsDryRun;
    public static readonly string OrganizationName;
    public static readonly string ProjectName;
    public static readonly string StackName;

    static StackEnv()
    {
        IsDryRun = Environment.GetEnvironmentVariable("PULUMI_DRY_RUN")!.Equals(Boolean.TrueString,
            StringComparison.OrdinalIgnoreCase);
        OrganizationName = Environment.GetEnvironmentVariable("PULUMI_ORGANIZATION")!;
        ProjectName = Environment.GetEnvironmentVariable("PULUMI_PROJECT")!;
        StackName = Environment.GetEnvironmentVariable("PULUMI_STACK")!;

        Console.WriteLine("Pulumi Environment:\n\tOrganization: {0}, \n\tProject: {1}, \n\tStack: {2}",
            OrganizationName,
            ProjectName,
            StackName);
    }
}