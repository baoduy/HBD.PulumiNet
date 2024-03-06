using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HBD.PulumiNet.Providers.Ssh;
using HBD.PulumiNet.Share.Core;
using HBD.PulumiNet.Share.KeyVaults;

public static class Stack
{
    public static async Task<Dictionary<string, object?>> RunAsync()
    {
        var resourceGroup = RgCreator.Create(new RgCreator.Args("test")).Info();

        var vault = await VaultCreator.Create(new VaultCreator.Args("test-vault", resourceGroup,
            []));

        var ssh  = new SshProviderResource("ssh", new SshArgs());

        ssh.PublicKey.Apply(s =>
        {
             Console.WriteLine(s);
             return string.Empty;
        });
        return new Dictionary<string, object?>();
    }
}