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

        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("read", "Read data"),
                new ApiScope("write", "Write data"),
                new ApiScope("delete", "Delete data")
            };


        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
               //cliente genérico
                new Client //1
                {
                    ClientId = "client",
                    ClientSecrets = { new Secret("abracadabra#simsalabim".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials, //precisa das credenciais do usuário
                    AllowedScopes = {"read", "write", "profile" }
                },

                // Cliente para Resource Owner Password
                new Client
                {
                    ClientId = "ro.client",
                    ClientSecrets = { new Secret("secreta".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "read", "write", "profile", "openid", "email" }
                }
            };

    }
}