using System.Collections.Generic;
using System.Threading.Tasks;
using HBD.PulumiNet.Share.Ad;
using HBD.PulumiNet.Share.Core;
using HBD.PulumiNet.Share.KeyVaults;

public static class Stack
{
    public static async Task<Dictionary<string, object?>> RunAsync()
    {
        var resourceGroup = RgCreator.Create(new RgCreator.Args("drunkcoding")).Info();

        var vault = await VaultCreator.Create(new VaultCreator.Args("vault", resourceGroup,
            new List<VaultCreator.PermissionsArgs>()));

        var app = AppRegister.Create(new AppRegister.Args("Test-Drunk", VaultInfo: vault.info));

        return new Dictionary<string, object?>();
    }
}