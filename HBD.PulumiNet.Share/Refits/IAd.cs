using Refit;

namespace HBD.PulumiNet.Share.Refits;

// public class RoleDefinitionProperties
// {
//     public string? RoleName { get; set; }
//     public string Type { get; set; } = "BuiltInRole";
//     public string? Description { get; set; }
// }

public class RoleDefinitionResult
{
    public string Name { get; set; }= null!;
    public string Id { get; set; }= null!;
    //public string? Type { get; set; }
    //public RoleDefinitionProperties Properties { get; set; } = new ();
};

public interface IAzureAd
{
    [Get("/providers/Microsoft.Authorization/roleDefinitions?$filter=roleName eq '{roleName}'&api-version=2015-07-01")]
    Task<AzResult<RoleDefinitionResult>> GetRoleDefinitionAsync(string roleName);
}