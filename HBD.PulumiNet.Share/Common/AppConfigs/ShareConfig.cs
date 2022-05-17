using HBD.PulumiNet.Share.Types;
using Pulumi;
// ReSharper disable UnusedMember.Global

namespace HBD.PulumiNet.Share.Common.AppConfigs;

public class ShareConfig
{
    public static readonly string Name = "Share";
    public static string ResourceGroupName = Name.GetResourceGroupName();
    public static readonly string LogResourceGroupName = $"{Name}-logs".GetResourceGroupName();

    public static readonly string InsightName = Name.GetAppInsightName();

    public static AzResourceInfo InsightInfo = new(InsightName, new ResourceGroupInfo(LogResourceGroupName),
        Output.Format(
            $"/subscriptions/${AzureEnv.SubscriptionId}/resourceGroups/${LogResourceGroupName}/providers/Microsoft.Insights/components/${InsightName}"));
}