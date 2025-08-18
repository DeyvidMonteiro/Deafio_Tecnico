using AutoMapper;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;
using DesafioTecnicoAvanade.EstoqueApi.DTOs;

namespace DesafioTecnicoAvanade.EstoqueApi.Services.Category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly ICategoryRideOnlyRepository _readRepository;
        private readonly ICategoryWriteOlyRepository _writeRepository;
        private readonly IMapper _mapper;

        public CategoryServices(ICategoryRideOnlyRepository readRepository, ICategoryWriteOlyRepository writeRepository, IMapper maper)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _mapper = maper;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategories()
        {
            var categories = await _readRepository.GetAll();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesProducts()
        {
            var categories = await _readRepository.GetCategoriesProducts();
            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            var category = await _readRepository.GetById(id);
            return _mapper.Map<CategoryDTO>(category);
        }
        public async Task AddCategory(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<EstoqueApi.Models.Category>(categoryDTO);
            await _writeRepository.Create(category);
            categoryDTO.Id = category.Id;
        }

        public async Task UpdateCategry(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<EstoqueApi.Models.Category>(categoryDTO);
            await _writeRepository.Update(category);
        }

        public async Task RemoveCategory(int id)
        {
            var category = await _readRepository.GetById(id);
            await _writeRepository.Delete(category.Id);
        }

    }
}
