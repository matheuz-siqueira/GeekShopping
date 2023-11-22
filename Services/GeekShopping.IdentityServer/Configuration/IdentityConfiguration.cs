using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace GeekShopping.IdentityServer.Configuration;

public static class IdentityConfiguration
{
    public const string Admin = "Admin";
    public const string Client = "Client";

    public static IEnumerable<IdentityResource> IdentityResources => 
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(), 
            new IdentityResources.Email(), 
            new IdentityResources.Profile()
        };
    
    public static IEnumerable<ApiScope> ApiScopes => 
        new List<ApiScope>
        {
            new("geek_shopping", "GeekShoppingServer"), 
            new(name: "read", "Read data."), 
            new(name: "white", "Write data."), 
            new(name: "delete", "Delete data.")
        };
    
    public static IEnumerable<Client> Clients => 
        new List<Client>
        {
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("Zjk5MDE5YmEtMjc3MC00ZjdhLThmY2QtNThjNWNlYTA5YjJh".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials, 
                AllowedScopes = { "read", "write", "profile"} 
            }, 

            new Client
            {
                ClientId = "geek_shopping",
                ClientSecrets = { new Secret("Zjk5MDE5YmEtMjc3MC00ZjdhLThmY2QtNThjNWNlYTA5YjJh".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"http://localhost:7031/signin-oidc"}, 
                PostLogoutRedirectUris = {"http://localhost:7031/signout-callback-oidc"}, 
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "geek_shopping"
                } 
            }
        };
}
