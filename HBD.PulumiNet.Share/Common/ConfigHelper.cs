namespace HBD.PulumiNet.Share.Common;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class ConfigHelper
{
    private static readonly Config _config = new();

    public static string GetValue(string name) => _config.Get(name);
    
    public static string RequireValue(string name) => _config.Require(name);
    
    public static Output<string>? GetSecret(string name) => _config.GetSecret(name);
    
    public static Output<string>? RequireSecret(string name) => _config.RequireSecret(name);
}