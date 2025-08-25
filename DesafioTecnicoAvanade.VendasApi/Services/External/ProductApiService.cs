using DesafioTecnicoAvanade.VendasApi.DTOs;

namespace DesafioTecnicoAvanade.VendasApi.Services.External
{
    public class ProductApiService : IProductApiService
    {
        private readonly HttpClient _httpClient;

        public ProductApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"/api/products/{productId}");

            if (!response.IsSuccessStatusCode)
            {
                return null; 
            }

            return await response.Content.ReadFromJsonAsync<ProductDTO>();
        }

        public async Task UpdateProductStockAsync(int productId, long quantityToDecrement)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/products/{productId}/decrement", quantityToDecrement);
            response.EnsureSuccessStatusCode();
        }
    }

}
