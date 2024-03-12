namespace HBD.PulumiNet.Common;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class ConfigHelper
{
    private static readonly Config Config = new();
    public static string? GetValue(string name) => Config.Get(name);
    public static string RequireValue(string name) => Config.Require(name);
    public static Output<string>? GetSecret(string name) => Config.GetSecret(name);
    public static Output<string>? RequireSecret(string name) => Config.RequireSecret(name);
}