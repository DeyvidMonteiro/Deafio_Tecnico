using DesafioTecnicoAvanade.EstoqueApi.Models;

namespace DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;

public interface IProductWriteOnlyRepository
{
    Task<Product> Create(Product product);
    Task<Product> Update(Product product);
    Task<Product> Delete(int id);
}
