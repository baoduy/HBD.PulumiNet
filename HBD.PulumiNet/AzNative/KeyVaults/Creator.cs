namespace HBD.PulumiNet.AzNative.KeyVaults;

public static class Creator
{
    public record Args(string Name, Action<string> Factory, AzResourceInfo VaultInfo);

    // public async Task<string> GetOrCreate(Args args, CancellationToken cancellationToken = default)
    // {
    //     var client = new Azure.Security.KeyVault.Secrets.SecretClient(vaultUri: new Uri($"https://{args.VaultInfo.Name}.vault.azure.net"),
    //         credential: new DefaultAzureCredential());
    //
    //     if(HBD.PulumiNet.KeyVaults.VaultsHelpers.)
    //     var secret = await client.GetSecretAsync(args.Name, cancellationToken: cancellationToken).ConfigureAwait(false);
    // }
}