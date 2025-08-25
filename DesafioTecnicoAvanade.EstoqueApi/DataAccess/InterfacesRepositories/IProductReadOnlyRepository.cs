using DesafioTecnicoAvanade.EstoqueApi.Models;

namespace DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;

public interface IProductReadOnlyRepository
{
    Task<IEnumerable<Product>> GetAll();
    Task<Product> GetById(int id);
    Task<Product> GetProductForUpdateAsync(int productId);
}
