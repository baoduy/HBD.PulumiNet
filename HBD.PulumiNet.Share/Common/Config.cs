using HBD.PulumiNet.Share.Types;

namespace HBD.PulumiNet.Share.Common;

public static class Config
{
    //This configuration will be shared across all the projects
    //TODO: Update this value when copy for other project

    //This will be added after the suffix of Resource Group
    //The project Name convention shall be az-Name, if using different name, please update the index below.
    public static readonly string OrganizationName = StackEnv.ProjectName.Split('-')[2];

    public static readonly string ApimHeaderKey = $"x-${OrganizationName.ToLower()}-key";
    public static readonly string ApimHookHeaderKey = $"x-${OrganizationName.ToLower()}-hook";

    public const string GlobalKeyName = "global";

    /**The Global resource group name.*/
    public static readonly ConventionArgs GlobalConvention =
        new(GlobalKeyName, string.IsNullOrEmpty(OrganizationName) ? "grp" : $"grp-{OrganizationName}");

    public static readonly ConventionArgs ResourceConvention = new(StackEnv.StackName.ToLower());

    //TODO: update default alert emails
    public static readonly string[] DefaultAlertEmails = new[] { "drunkcoding@outlook.com" };
}