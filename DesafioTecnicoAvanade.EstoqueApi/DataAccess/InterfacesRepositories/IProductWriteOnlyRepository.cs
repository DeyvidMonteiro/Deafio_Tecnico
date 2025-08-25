using DesafioTecnicoAvanade.EstoqueApi.Models;

namespace DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;

public interface IProductWriteOnlyRepository
{
    Task<Product> Create(Product product);
    Task<Product> Update(Product product);
    Task Decrement(int productId, long quantity);
    Task<Product> Delete(int id);
}
