using HBD.PulumiNet.Share.KeyVaults;
using Pulumi.AzureAD;
using Pulumi.AzureAD.Inputs;
using ApplicationAppRoleArgs = Pulumi.AzureAD.Inputs.ApplicationAppRoleArgs;
namespace HBD.PulumiNet.Share.AzAd;

/// <summary>
/// Synced 24/Jan/23
/// </summary>
public static class AppRegister
{
    public record Args(string Name,
        InputList<string>? RedirectUris = null,
        AzResourceInfo? VaultInfo = null,
        Input<string>? HomepageUrl = null,
        Input<string>? LogoutUrl = null,
        InputList<string>? Owners = null,
        bool AllowImplicit = false, 
        bool AllowMultiOrg = false, 
        InputList<ApplicationAppRoleArgs>? AppRoles = null,
        InputList<ApplicationApiOauth2PermissionScopeArgs>? Oauth2PermissionScopes = null);

    public record Result(Output<string> ClientId, Output<string> ClientSecret);

    public static Result Create(Args args)
    {
        // Azure AD Application no need suffix
        var name = args.Name.GetIdentityName();

        var clientIdKeyName = $"{name}-client-id";
        var clientSecretKeyName = $"{name}-client-secret";
        var principalIdKeyName = $"{name}-principal-id";
        var principalSecretKeyName = $"{name}-principal-secret";

        var parameters = new ApplicationArgs
        {
            DisplayName = name,
            Owners = args.Owners,
            AppRoles = args.AppRoles ?? [],
            SignInAudience = args.AllowMultiOrg ? "AzureADMultipleOrgs" : "AzureADMyOrg",
            GroupMembershipClaims = "SecurityGroup",
        };

        if (args.AllowImplicit)
        {
            //SPA
            parameters.SinglePageApplication = new ApplicationSinglePageApplicationArgs
                {RedirectUris = args.RedirectUris ?? [] };
            //Web
            parameters.Web = new ApplicationWebArgs
            {
                HomepageUrl = args.HomepageUrl,
                ImplicitGrant = new ApplicationWebImplicitGrantArgs
                    {AccessTokenIssuanceEnabled = true, IdTokenIssuanceEnabled = true},
                RedirectUris = args.RedirectUris ?? [],
                LogoutUrl = args.LogoutUrl
            };
        }
        else
        {
            parameters.Api = new ApplicationApiArgs
            {
                RequestedAccessTokenVersion = 2,
                Oauth2PermissionScopes = args.Oauth2PermissionScopes ??
                                         []
            };
        }

        var app = new Application(name, parameters);

        var appPass = new ApplicationPassword(name, new ApplicationPasswordArgs
                {DisplayName = name, 
                    ApplicationObjectId = app.ApplicationId, 
                    EndDateRelative = "43800h"},
            new CustomResourceOptions {DependsOn = app});

        var principal = new ServicePrincipal(name,
            new ServicePrincipalArgs {Description = name, 
                ApplicationId = app.ApplicationId, 
                Owners = args.Owners},
            new CustomResourceOptions {DependsOn = app});

        var principalPass = new ServicePrincipalPassword(name,
            new ServicePrincipalPasswordArgs {ServicePrincipalId = principal.ObjectId},
            new CustomResourceOptions {DependsOn = principal});

        if (args.VaultInfo != null)
        {
            VaultsHelpers.AddSecret(new VaultsHelpers.SecretArgs(clientIdKeyName, app.ApplicationId, args.VaultInfo,
                $"{name} ClientId", DependsOn: app));
            VaultsHelpers.AddSecret(new VaultsHelpers.SecretArgs(clientSecretKeyName, appPass.Value, args.VaultInfo,
                $"{name} ClientSecret", DependsOn: appPass));
            VaultsHelpers.AddSecret(new VaultsHelpers.SecretArgs(principalIdKeyName, principal.ObjectId, args.VaultInfo,
                $"{name} PrincipalId", DependsOn: principal));
            VaultsHelpers.AddSecret(new VaultsHelpers.SecretArgs(principalSecretKeyName, principalPass.Value, args.VaultInfo,
                $"{name} PrincipalSecret", DependsOn: principalPass));
        }

        return new Result(app.ApplicationId, appPass.Value);
    }
}