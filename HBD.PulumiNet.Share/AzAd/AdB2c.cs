using Pulumi.AzureNative.AzureActiveDirectory;
using Pulumi.AzureNative.AzureActiveDirectory.Inputs;

namespace HBD.PulumiNet.Share.AzAd;

public static class AdB2C
{
    public enum Locations
    {
        SG = 1 << 0,
        US = 1 << 1,
        AU = 1 << 2,
        EU = 1 << 3
    }

    public record Args(string Name, ResourceGroupInfo Group) : BasicArgs(Name, Group)
    {
        public string? DisplayName { get; init; }
        public Locations Location { get; init; }
    }

    private static string GetLocationName(Locations location) =>
        location switch
        {
            Locations.US => "United States",
            Locations.AU => "Australia",
            Locations.EU => "Europe",
            Locations.SG => "Asia Pacific",
            _ => "Asia Pacific"
        };

    public static void Create(Args args)
    {
        var name = args.Name.GetB2cName();
        var b2cTenant = new B2CTenant(name, new B2CTenantArgs
        {
            ResourceName = name,
            ResourceGroupName = args.Group.ResourceGroupName,
            Location = GetLocationName(args.Location),

            DisplayName = args.DisplayName ?? name, 
            CountryCode = args.Location.ToString(),
            
            Sku = new B2CResourceSKUArgs
            {
                Name = B2CResourceSKUName.Standard,
                Tier = B2CResourceSKUTier.A0
            }
        });

        if (args.Lock)
        {
            
        }
    }
}