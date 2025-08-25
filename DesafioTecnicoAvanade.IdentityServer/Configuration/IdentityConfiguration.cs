using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace DesafioTecnicoAvanade.IdentityServer.Configuration
{
    public class IdentityConfiguration
    {
        public const string Admin = "Admin";
        public const string Client = "Client";

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            // ... (escopos 'read', 'write', 'delete' mantidos)
            new ApiScope("possivelGateway", "Acesso ao possível gateway")
        };

        public static IEnumerable<Client> Clients => new List<Client>
            {
            // Cliente para comunicação de máquina para máquina (por exemplo, uma API chamando outra)
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("4FjL@#kRz!9pW$vQyH*m7E%sA^dJ&c5tG".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"read", "write"}
            },

            // Cliente para testes (usado com Postman/Swagger UI)
            new Client
            {
                ClientId = "ro.client",
                ClientSecrets = { new Secret("secreto".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = { "openid", "profile", "email", "read", "write", "delete" }
            },

            // Cliente para o frontend (aplicação web)
            new Client
            {
                ClientId = "possivelGateway",
                ClientSecrets = { new Secret("4FjL@#kRz!9pW$vQyH*m7E%sA^dJ&c5tG".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"https://localhost:7165/signin-oidc"},
                PostLogoutRedirectUris = {"https://localhost:7165/signout-callback-oidc"},
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "possivelGateway"
                }
            }
        };

    }
}