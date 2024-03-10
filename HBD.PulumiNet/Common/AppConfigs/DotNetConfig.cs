namespace HBD.PulumiNet.Common.AppConfigs;

/// <summary>
/// Synced on 24/Jan/23
/// </summary>
public class DotNetConfig
{
    public static Dictionary<string, string> DefaultConfig = new()
    {
        ["COMPlus_EnableDiagnostics"] = "0",
        ["ASPNETCORE_URLS"] = "http://*:8080",
        //ASPNETCORE_ENVIRONMENT: "Production",
        ["AllowedHosts"] = "*",

        ["Logging__LogLevel__Default"] = "Information",
        ["Logging__LogLevel__System"] = "Error",
        ["Logging__LogLevel__Microsoft"] = "Error",
        ["Console__IncludeScopes"] = "false",
        ["Console__LogLevel__Default"] = "Information",
        ["Console__LogLevel__System"] = "Error",
        ["Console__LogLevel__Microsoft"] = "Error",
        ["Debug__LogLevel__Default"] = "None",
        ["Debug__LogLevel__System"] = "None",
        ["Debug__LogLevel__Microsoft"] = "None"
    };
}