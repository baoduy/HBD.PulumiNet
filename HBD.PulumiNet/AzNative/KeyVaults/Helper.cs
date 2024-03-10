using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace HBD.PulumiNet.AzNative.KeyVaults;

public static class Helper
{
    public static async Task<bool> IsSecretExists(string name, AzResourceInfo vaultInfo, CancellationToken cancellationToken = default)
    {
        var client = new SecretClient(vaultUri: new Uri($"https://{vaultInfo.Name}.vault.azure.net"), credential: new DefaultAzureCredential());

        await foreach (var v in client.GetPropertiesOfSecretVersionsAsync(name, cancellationToken).AsPages(pageSizeHint: 1)
                           .WithCancellation(cancellationToken).ConfigureAwait(false))
            return v.Values.Count > 0;
        return false;
    }
}