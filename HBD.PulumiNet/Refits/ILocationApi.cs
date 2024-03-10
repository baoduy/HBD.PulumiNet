using Refit;

namespace HBD.PulumiNet.Refits;

public class LocationResult {
    public string Name { get; set; }= null!;
    public string DisplayName{ get; set; }= null!;
}

public interface ILocationApi
{
    [Get("/locations?api-version=2020-01-01")]
    Task<AzResult<LocationResult>> GetAsync();
}