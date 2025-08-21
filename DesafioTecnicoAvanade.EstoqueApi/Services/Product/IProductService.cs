using DesafioTecnicoAvanade.EstoqueApi.DTOs;

namespace DesafioTecnicoAvanade.EstoqueApi.Services.Product;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProducts();
    Task<ProductDTO> GetProductById(int id);
    Task AddProduct(ProductDTO productDTO);
    Task Updateproduct(ProductDTO productDTO);
    Task RemoveProduct(int id);
    Task DecrementStock(int productId, long quantity);
}
