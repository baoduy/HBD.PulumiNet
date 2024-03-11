using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HBD.PulumiNet.AzAd;
using HBD.PulumiNet.AzNative.KeyVaults;
using HBD.PulumiNet.Common;
using HBD.PulumiNet.Core;
using HBD.PulumiNet.KeyVaults;

public static class Stack
{
    public static async Task<Dictionary<string, object?>> RunAsync()
    {
        EnvRoles.CreateEnvRoles(AzureEnv.CurrentEnv);

        var resourceGroup = RgCreator.Create(new RgCreator.Args("test")).Info();

        var vault = VaultCreator.Create(new VaultCreator.Args("test-vault", resourceGroup,
            Auth: EnvRoles.GetEnvRoles(AzureEnv.CurrentEnv)));

        Console.WriteLine(await Creator.GetOrCreateSecret(new Creator.Args("Steven", () => "AAA", vault.info)));

        return new Dictionary<string, object?>();
    }
}