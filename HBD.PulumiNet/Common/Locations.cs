using HBD.PulumiNet.Refits;

namespace HBD.PulumiNet.Common;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class Locations
{
    private static IList<LocationResult>? _locationCache;

    public static async Task<IList<LocationResult>> GetAllLocationsAsync()
    {
        if (_locationCache != null) return _locationCache;

        var api = await Factory.Create<ILocationApi>().ConfigureAwait(false);
        var result = await api.GetAsync().ConfigureAwait(false);
        _locationCache = result.Value;

        return _locationCache;
    }

    public static async Task<string> GetLocationString(string possibleName)
    {
        var locations = await GetAllLocationsAsync().ConfigureAwait(false);
        var location = locations.FirstOrDefault(
            l => string.Equals(l.Name, possibleName, StringComparison.OrdinalIgnoreCase));

        return location?.DisplayName ?? AzureEnv.CurrentLocation;
    }

    public static Output<string> GetLocation(Input<string> possibleName) => possibleName.Apply(GetLocationString);
}