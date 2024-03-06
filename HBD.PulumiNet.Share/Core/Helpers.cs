using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.Insights;
using Pulumi.AzureNative.Insights.Inputs;
using Pulumi.AzureNative.Security;

// ReSharper disable UseDeconstructionOnParameter

namespace HBD.PulumiNet.Share.Core;

public static class Helpers
{
    public static TResource EnableLock<TResource>(this TResource resource, LockLevel? level = null, bool protect = true)
        where TResource : CustomResource
    {
        level ??= LockLevel.CanNotDelete;
        var name = $"{resource.GetResourceName()}-{level}-lock";
        var resourceId = resource.Id;

        var _ = new ManagementLockByScope(name,
            new ManagementLockByScopeArgs()
            {
                LockName = name,
                Level = level,
                Scope = resourceId,
                Notes = $"Lock {name} from {level}",
            }, new CustomResourceOptions {DependsOn = resource, Protect = protect});

        return resource;
    }

    public class LogDestinationArgs
    {
        public Input<string>? WorkspaceId { get; set; }
        public Input<string>? StorageAccountId { get; set; }
    }

    public static TResource EnableDiagnostic<TResource>(this TResource resource, LogDestinationArgs destination,
        IEnumerable<string>? metrics = null, IEnumerable<string>? logs = null) where TResource : CustomResource
    {
        var name = $"{resource.GetResourceName()}-diag";
        var resourceId = resource.Id;

        var _ =new DiagnosticSetting(name, new DiagnosticSettingArgs
        {
            Name = name,
            ResourceUri = resourceId,
            LogAnalyticsDestinationType = "AzureDiagnostics",
            WorkspaceId = destination.WorkspaceId,
            StorageAccountId = destination.StorageAccountId,
            
            Logs = logs?.Select(l => new LogSettingsArgs
                {
                    Category = l, Enabled = true, RetentionPolicy = new RetentionPolicyArgs {Days = 7, Enabled = true}
                })
                .ToList() ?? new InputList<LogSettingsArgs>(),
            Metrics = metrics?.Select(m => new MetricSettingsArgs
                {
                    Category = m, Enabled = true, RetentionPolicy = new RetentionPolicyArgs {Days = 7, Enabled = true}
                })
                .ToList() ?? new InputList<MetricSettingsArgs>()
        });

        return resource;
    }

    public static TResource EnableThreatProtection<TResource>(this TResource resource)where TResource : CustomResource
    {
        var name = $"{resource.GetResourceName()}-threat";
        var resourceId = resource.Id;

        
        var _= new AdvancedThreatProtection(name, new AdvancedThreatProtectionArgs{
            IsEnabled = true,
            ResourceId = resourceId,
            SettingName = name
        });
        
        return resource;
    }
}