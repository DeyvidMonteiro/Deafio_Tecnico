using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Services.External.Contracts;

namespace DesafioTecnicoAvanade.VendasApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string? _accessToken;
        private DateTime _tokenExpiration;
        private readonly IConfiguration _configuration;

        public TokenService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;

        }

        public async Task<string> GetAccessToken()
        {
            if (!string.IsNullOrEmpty(_accessToken) && _tokenExpiration > DateTime.Now)
            {
                return _accessToken;
            }

            var client = _httpClientFactory.CreateClient("IdentityApi");

            var loginData = new
            {
                UserName = _configuration["ApiCredentials:UserName"],
                Password = _configuration["ApiCredentials:Password"],
            };

            var response = await client.PostAsJsonAsync("/auth/login", loginData);

            response.EnsureSuccessStatusCode();

            var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponseModel>();

            _accessToken = tokenResponse.Token;
            _tokenExpiration = tokenResponse.Expiration;

            return _accessToken;
        }
    }

}