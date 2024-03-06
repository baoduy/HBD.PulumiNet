using HBD.PulumiNet.Share.Refits;

namespace HBD.PulumiNet.Share.Common;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class Locations
{
    private static IList<LocationResult>? _locationCache;

    public static async Task<IList<LocationResult>> GetAllLocationsAsync()
    {
        if (_locationCache != null) return _locationCache;

        var api = await Factory.Create<ILocationApi>();
        var result = await api.GetAsync();
        _locationCache = result.Value;

        return _locationCache;
    }

    public static async Task<string> GetLocationString(string possibleName)
    {
        var locations = await GetAllLocationsAsync();
        var location = locations.FirstOrDefault(
            l => string.Equals(l.Name, possibleName, StringComparison.OrdinalIgnoreCase));

        return location?.DisplayName ?? AzureEnv.CurrentLocation;
    }

    public static Output<string> GetLocation(Input<string> possibleName) => possibleName.Apply(GetLocationString);
}