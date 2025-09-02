using Moq;
using Xunit;
using AutoMapper;
using DesafioTecnicoAvanade.EstoqueApi.Models;
using DesafioTecnicoAvanade.EstoqueApi.DTOs;
using DesafioTecnicoAvanade.EstoqueApi.Services.Category;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;

public class CategoryServicesTests
{
    private readonly Mock<ICategoryRideOnlyRepository> _mockReadRepository;
    private readonly Mock<ICategoryWriteOlyRepository> _mockWriteRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly CategoryServices _categoryService;

    public CategoryServicesTests()
    {
        _mockReadRepository = new Mock<ICategoryRideOnlyRepository>();
        _mockWriteRepository = new Mock<ICategoryWriteOlyRepository>();
        _mockMapper = new Mock<IMapper>();

        _categoryService = new CategoryServices(
            _mockReadRepository.Object,
            _mockWriteRepository.Object,
            _mockMapper.Object);
    }

    [Fact]
    public async Task GetCategories()
    {
        // Arrange
        var categories = new List<Category> { new Category { Id = 1, Name = "Categoria A" },
            new Category { Id = 2, Name = "Categoria B" } };

        var categoryDtos = new List<CategoryDTO> { new CategoryDTO { Id = 1, Name = "Categoria A" },
            new CategoryDTO { Id = 2, Name = "Categoria B" } };

        _mockReadRepository.Setup(r => r.GetAll()).ReturnsAsync(categories);
        _mockMapper.Setup(m => m.Map<IEnumerable<CategoryDTO>>(categories)).Returns(categoryDtos);

        // Act
        var result = await _categoryService.GetCategories();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetCategoriesProducts()
    {
        // Arrange
        var categoriesWithProducts = new List<Category> { new Category { Id = 1, Name = "Eletrônicos" } };
        var categoryDtos = new List<CategoryDTO> { new CategoryDTO { Id = 1, Name = "Eletrônicos" } };

        _mockReadRepository.Setup(r => r.GetCategoriesProducts()).ReturnsAsync(categoriesWithProducts);
        _mockMapper.Setup(m => m.Map<IEnumerable<CategoryDTO>>(categoriesWithProducts)).Returns(categoryDtos);

        // Act
        var result = await _categoryService.GetCategoriesProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetCategoryById()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId, Name = "Ferramentas" };
        var categoryDto = new CategoryDTO { Id = categoryId, Name = "Ferramentas" };

        _mockReadRepository.Setup(r => r.GetById(categoryId)).ReturnsAsync(category);
        _mockMapper.Setup(m => m.Map<CategoryDTO>(category)).Returns(categoryDto);

        // Act
        var result = await _categoryService.GetCategoryById(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Id);
    }

    [Fact]
    public async Task AddCategory()
    {
        // Arrange
        var categoryDto = new CategoryDTO { Name = "Livros" };
        var category = new Category { Name = "Livros" };
        var newCategoryId = 1;

        _mockMapper.Setup(m => m.Map<Category>(categoryDto)).Returns(category);
        _mockWriteRepository.Setup(w => w.Create(category)).Callback(() => category.Id = newCategoryId);

        // Act
        await _categoryService.AddCategory(categoryDto);

        // Assert
        _mockWriteRepository.Verify(w => w.Create(category), Times.Once);
        Assert.Equal(newCategoryId, categoryDto.Id);
    }

    [Fact]
    public async Task UpdateCategory()
    {
        // Arrange
        var categoryDto = new CategoryDTO { Id = 1, Name = "Esportes" };
        var category = new Category { Id = 1, Name = "Esportes" };

        _mockMapper.Setup(m => m.Map<Category>(categoryDto)).Returns(category);

        // Act
        await _categoryService.UpdateCategry(categoryDto);

        // Assert
        _mockWriteRepository.Verify(w => w.Update(category), Times.Once);
    }

    [Fact]
    public async Task RemoveCategory()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId };

        _mockReadRepository.Setup(r => r.GetById(categoryId)).ReturnsAsync(category);

        // Act
        await _categoryService.RemoveCategory(categoryId);

        // Assert
        _mockWriteRepository.Verify(w => w.Delete(categoryId), Times.Once);
    }
}