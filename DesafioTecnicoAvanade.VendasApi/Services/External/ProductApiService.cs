using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Services.External.Contracts;

namespace DesafioTecnicoAvanade.VendasApi.Services.External
{
    public class ProductApiService : IProductApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public ProductApiService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        private async Task AddAuthorizationHeader()
        {
            var token = await _tokenService.GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            await AddAuthorizationHeader();

            var response = await _httpClient.GetAsync($"products/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return null; 
            }

            return await response.Content.ReadFromJsonAsync<ProductDTO>();
        }

        public async Task UpdateProductStockAsync(int productId, int quantityToDecrement)
        {
            await AddAuthorizationHeader();

            var response = await _httpClient.PutAsJsonAsync($"products/{productId}/decrement", quantityToDecrement);
            response.EnsureSuccessStatusCode();
        }
    }

}
