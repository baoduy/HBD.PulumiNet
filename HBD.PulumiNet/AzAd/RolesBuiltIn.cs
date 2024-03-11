using System.Text.Json;
using System.Text.Json.Serialization;

namespace HBD.PulumiNet.AzAd;

public static class RolesBuiltIn
{
    public sealed class AzRoleProperty
    {
        [JsonPropertyName("roleName")]
        public string Name { get; set; } = string.Empty;
    }
    public sealed class AzRole
    {
        [JsonPropertyName("id")]
        public string Scope { get; set; } = default!;
        
        [JsonPropertyName("name")]
        public string Id { get; set; } = default!;
        
        [JsonPropertyName("properties")]
        public AzRoleProperty Properties { get; set; } = default!;
    }

    private static List<AzRole> BuiltInRoles { get; }

    static RolesBuiltIn()
    {
        // Get the assembly that contains the resource
        var assembly = typeof(RolesBuiltIn).Assembly;
        const string resourceName = "HBD.PulumiNet.AzAd.RolesBuiltIn.json";

        // Obtain a stream to the resource
        using var stream = assembly.GetManifestResourceStream(resourceName);
        BuiltInRoles = stream != null ? JsonSerializer.Deserialize<List<AzRole>>(stream)! : [];
    }

    public static AzRole? Find(string name) 
        => BuiltInRoles.FirstOrDefault(i => i.Properties.Name.Contains(name));
}