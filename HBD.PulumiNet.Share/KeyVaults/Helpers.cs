using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;

namespace HBD.PulumiNet.Share.KeyVaults;

public static class VaultsHelpers
{
    public record SecretArgs(string Name, 
        Input<string> Value, 
        AzResourceInfo VaultInfo,
        Input<string>? ContentType = default,
        InputMap<string>? Tags = null, 
        InputList<Resource>? DependsOn =null);

    public static Secret? AddSecret(SecretArgs args)
    {
        var name = args.Name.GetSecretName();

        //TODO: Check if it is not dry run and call Key Vault API to recover deleted secret if needed.
        return new Secret(name,
            new Pulumi.AzureNative.KeyVault.SecretArgs
            {
                SecretName = name,
                Tags = args.Tags ?? [],
                Properties = new SecretPropertiesArgs
                    { Value = args.Value, ContentType = args.ContentType },
                VaultName = args.VaultInfo.Name,
                ResourceGroupName = args.VaultInfo.Group.ResourceGroupName,
                
            }, new CustomResourceOptions{DependsOn = args.DependsOn?? [] });
    }

    public record KeyArgs(string Name, AzResourceInfo VaultInfo, InputMap<string>? Tags = null);

    public static Key AddKey(KeyArgs args)
    {
        var name = args.Name.GetSecretName();

        //TODO: Check if it is not dry run and call Key Vault API to recover deleted secret if needed.
        return new Key(name,
            new Pulumi.AzureNative.KeyVault.KeyArgs
            {
                KeyName = name,
                Tags = args.Tags ?? [],
                Properties = new KeyPropertiesArgs
                {
                    KeySize = 2048,
                    Kty = "RSA",
                    KeyOps =
                    [
                        "decrypt",
                        "encrypt",
                        "sign",
                        "verify",
                        "wrapKey",
                        "unwrapKey"
                    ],
                    //curveName: 'P512',
                    Attributes = new KeyAttributesArgs { Enabled = true },
                },
                VaultName = args.VaultInfo.Name,
                ResourceGroupName = args.VaultInfo.Group.ResourceGroupName,
            });
    }
}