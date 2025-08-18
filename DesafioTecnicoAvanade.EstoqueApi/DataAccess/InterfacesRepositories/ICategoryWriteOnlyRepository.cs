using DesafioTecnicoAvanade.EstoqueApi.Models;

namespace DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;

public interface ICategoryWriteOlyRepository
{
    Task<Category> Create(Category category);
    Task<Category> Update(Category category);
    Task<Category> Delete(int id);
}
