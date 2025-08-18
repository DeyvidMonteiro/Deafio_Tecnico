using DesafioTecnicoAvanade.EstoqueApi.Models;

namespace DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories
{
    public interface ICategoryRideOnlyRepository
    {
        Task<IEnumerable<Category>> GetAll();
        Task<IEnumerable<Category>> GetCategoriesProducts();
        Task<Category> GetById(int id);
    }
}
