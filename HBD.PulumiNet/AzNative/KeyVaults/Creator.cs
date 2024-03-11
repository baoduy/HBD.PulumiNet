using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace HBD.PulumiNet.AzNative.KeyVaults;

public static class Creator
{
    public record Args(string Name, Func<string> Factory, AzResourceInfo VaultInfo);

    public static async Task<KeyVaultSecret> GetOrCreateSecret(Args args, CancellationToken cancellationToken = default)
    {
        var client = new SecretClient(
            vaultUri: new Uri($"https://{args.VaultInfo.Name}.vault.azure.net"),
            credential: new DefaultAzureCredential());

        //Already created
        if (await Helper.IsSecretExists(args.Name, vaultInfo: args.VaultInfo, cancellationToken: cancellationToken)
                .ConfigureAwait(false))
        {
            Console.WriteLine($"The secret: {args.Name} already existed. Will load again from the vault.");
            return (await client.GetSecretAsync(args.Name, cancellationToken: cancellationToken)
                .ConfigureAwait(false)).Value;
        }

        //Create
        var rs = await client.SetSecretAsync(new KeyVaultSecret(args.Name, args.Factory())
            { Properties = { Enabled = true } }, cancellationToken).ConfigureAwait(false);

        return rs.Value;
    }
}