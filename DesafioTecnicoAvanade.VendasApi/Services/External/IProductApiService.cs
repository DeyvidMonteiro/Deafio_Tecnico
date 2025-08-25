using DesafioTecnicoAvanade.VendasApi.DTOs;

namespace DesafioTecnicoAvanade.VendasApi.Services.External
{
    public interface IProductApiService
    {
        Task<ProductDTO> GetProductByIdAsync(int productId);
        Task UpdateProductStockAsync(int productId, long newStock);
    }
}
