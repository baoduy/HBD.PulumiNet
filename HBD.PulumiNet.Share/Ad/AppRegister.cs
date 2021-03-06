using HBD.PulumiNet.Share.Common;
using HBD.PulumiNet.Share.Types;
using Pulumi;
using Pulumi.AzureAD;
using Pulumi.AzureAD.Inputs;
using Helpers = HBD.PulumiNet.Share.KeyVaults.Helpers;

namespace HBD.PulumiNet.Share.Ad;

public static class AppRegister
{
    public record Args(string Name,
        InputList<string>? RedirectUris = null,
        AzResourceInfo? VaultInfo = null,
        Input<string>? HomepageUrl = null,
        Input<string>? LogoutUrl = null,
        bool AllowImplicit = false, bool AllowMultiOrg = false, InputList<ApplicationAppRoleArgs>? AppRoles = null,
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
            Owners = AzureEnv.DefaultOwners,
            AppRoles = args.AppRoles ?? new InputList<ApplicationAppRoleArgs>(),
            SignInAudience = args.AllowMultiOrg ? "AzureADMultipleOrgs" : "AzureADMyOrg",
            GroupMembershipClaims = "SecurityGroup",
        };

        if (args.AllowImplicit)
        {
            //SPA
            parameters.SinglePageApplication = new ApplicationSinglePageApplicationArgs
                {RedirectUris = args.RedirectUris ?? new InputList<string>()};
            //Web
            parameters.Web = new ApplicationWebArgs
            {
                HomepageUrl = args.HomepageUrl,
                ImplicitGrant = new ApplicationWebImplicitGrantArgs
                    {AccessTokenIssuanceEnabled = true, IdTokenIssuanceEnabled = true},
                RedirectUris = args.RedirectUris ?? new InputList<string>(),
                LogoutUrl = args.LogoutUrl
            };
        }
        else
        {
            parameters.Api = new ApplicationApiArgs
            {
                RequestedAccessTokenVersion = 2,
                Oauth2PermissionScopes = args.Oauth2PermissionScopes ??
                                         new InputList<ApplicationApiOauth2PermissionScopeArgs>()
            };
        }

        var app = new Application(name, parameters);

        var appPass = new ApplicationPassword(name, new ApplicationPasswordArgs
                {DisplayName = name, ApplicationObjectId = app.ApplicationId, EndDateRelative = "43800h"},
            new CustomResourceOptions {DependsOn = app});

        var principal = new ServicePrincipal(name,
            new ServicePrincipalArgs {Description = name, ApplicationId = app.ApplicationId, Owners = AzureEnv.DefaultOwners},
            new CustomResourceOptions {DependsOn = app});

        var principalPass = new ServicePrincipalPassword(name,
            new ServicePrincipalPasswordArgs {ServicePrincipalId = principal.ObjectId},
            new CustomResourceOptions {DependsOn = principal});

        if (args.VaultInfo != null)
        {
            Helpers.AddSecret(new Helpers.SecretArgs(clientIdKeyName, app.ApplicationId, args.VaultInfo,
                $"{name} ClientId", DependsOn: app));
            Helpers.AddSecret(new Helpers.SecretArgs(clientSecretKeyName, appPass.Value, args.VaultInfo,
                $"{name} ClientSecret", DependsOn: appPass));
            Helpers.AddSecret(new Helpers.SecretArgs(principalIdKeyName, principal.ObjectId, args.VaultInfo,
                $"{name} PrincipalId", DependsOn: principal));
            Helpers.AddSecret(new Helpers.SecretArgs(principalSecretKeyName, principalPass.Value, args.VaultInfo,
                $"{name} PrincipalSecret", DependsOn: principalPass));
        }

        return new Result(app.ApplicationId, appPass.Value);
    }
}