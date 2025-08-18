using DesafioTecnicoAvanade.EstoqueApi.DTOs;

namespace DesafioTecnicoAvanade.EstoqueApi.Services.Category;

public interface ICategoryServices
{
    Task<IEnumerable<CategoryDTO>> GetCategories();
    Task<IEnumerable<CategoryDTO>> GetCategoriesProducts();
    Task<CategoryDTO> GetCategoryById(int id);
    Task AddCategory(CategoryDTO categoryDTO);
    Task UpdateCategry(CategoryDTO categoryDTO);
    Task RemoveCategory(int id);
}
