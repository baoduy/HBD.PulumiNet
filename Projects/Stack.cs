using System.Collections.Generic;
using System.Threading.Tasks;
using HBD.PulumiNet.Core;
using HBD.PulumiNet.KeyVaults;
using Console = System.Console;

public static class Stack
{
    public static async Task<Dictionary<string, object?>> RunAsync()
    {
        var resourceGroup = RgCreator.Create(new RgCreator.Args("test")).Info();

        var vault = await VaultCreator.Create(new VaultCreator.Args("test-vault", resourceGroup,
            []));

        Console.WriteLine(await HBD.PulumiNet.AzNative.KeyVaults.Helper.IsSecretExists("Steven", vault.info));

        return new Dictionary<string, object?>();
    }
}