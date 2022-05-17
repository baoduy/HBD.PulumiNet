using HBD.PulumiNet.Share.Ad;
using Pulumi;
using Pulumi.AzureNative.Authorization;
using Pulumi.AzureNative.KeyVault;
using Pulumi.AzureNative.KeyVault.Inputs;

namespace HBD.PulumiNet.Share.KeyVaults;

public static class Permissions
{
    public static readonly PermissionsArgs KeyVaultAdminPolicy = new()
    {
        Certificates = new InputList<Union<string, CertificatePermissions>> {CertificatePermissions.All},
        Keys = new InputList<Union<string, KeyPermissions>>{KeyPermissions.All},
        Secrets = new InputList<Union<string, SecretPermissions>>{SecretPermissions.All},
        Storage = new InputList<Union<string, StoragePermissions>>{StoragePermissions.All}
    };
    
    public static readonly PermissionsArgs KeyVaultReadOnlyPolicy = new()
    {
        Certificates = new InputList<Union<string, CertificatePermissions>> {CertificatePermissions.Get,CertificatePermissions.List},
        Keys = new InputList<Union<string, KeyPermissions>>{KeyPermissions.Get,KeyPermissions.List,KeyPermissions.Decrypt,KeyPermissions.Encrypt,KeyPermissions.Sign,KeyPermissions.UnwrapKey,KeyPermissions.Verify,KeyPermissions.WrapKey},
        Secrets = new InputList<Union<string, SecretPermissions>>{SecretPermissions.Get,SecretPermissions.List},
        Storage = new InputList<Union<string, StoragePermissions>>{StoragePermissions.Get,StoragePermissions.List}
    };
    
    public enum Type
    {
        ReadOnly = 0,
        ReadWrite = 1
    }
    
    public record Args(string Name,Input<string> PrincipalId, PrincipalType PrincipalType, Type Permission,Vault Vault);
    
    public static async Task GrantVaultRbacPermission (Args args){
        var vn = $"{args.Name}-{args.Permission}".ToLower();

        //ReadOnly
        if (args.Permission == Type.ReadOnly)
        {
            await RoleAssignments.Create(new RoleAssignments.Args($"{vn}-encrypt","Key Vault Crypto Service Encryption User",args.PrincipalId,args.PrincipalType,args.Vault.Id,args.Vault));
            await RoleAssignments.Create(new RoleAssignments.Args($"{vn}-crypto","Key Vault Crypto User",args.PrincipalId,args.PrincipalType,args.Vault.Id,args.Vault));
            await RoleAssignments.Create(new RoleAssignments.Args($"{vn}-secret","Key Vault Secrets User",args.PrincipalId,args.PrincipalType,args.Vault.Id,args.Vault));
            
            //Read and Write
        } else {
            await RoleAssignments.Create(new RoleAssignments.Args($"{vn}-cert","Key Vault Certificates Officer",args.PrincipalId,args.PrincipalType,args.Vault.Id,args.Vault));
            await RoleAssignments.Create(new RoleAssignments.Args($"{vn}-crypto","Key Vault Crypto Officer",args.PrincipalId,args.PrincipalType,args.Vault.Id,args.Vault));
            await RoleAssignments.Create(new RoleAssignments.Args($"{vn}-secret","Key Vault Secrets Officer",args.PrincipalId,args.PrincipalType,args.Vault.Id,args.Vault));
        }
    }
}