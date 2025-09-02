using DesafioTecnicoAvanade.VendasApi.DTOs;

namespace DesafioTecnicoAvanade.VendasApi.Services.External.Contracts
{
    public interface IProductApiService
    {
        Task<ProductDTO> GetProductByIdAsync(int productId);
        Task UpdateProductStockAsync(int productId, int quantityToDecrement);
    }
}
