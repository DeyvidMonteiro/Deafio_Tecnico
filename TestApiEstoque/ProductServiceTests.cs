using Moq;
using Xunit;
using AutoMapper;
using DesafioTecnicoAvanade.EstoqueApi.Models;
using DesafioTecnicoAvanade.EstoqueApi.DTOs;
using DesafioTecnicoAvanade.EstoqueApi.Services.Product;
using DesafioTecnicoAvanade.EstoqueApi.Filters.Exceptions;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.UnitOfWork;
using DesafioTecnicoAvanade.EstoqueApi.DataAccess.InterfacesRepositories;
using Microsoft.Extensions.Logging;

public class ProductServiceTests
{
    private readonly Mock<IProductReadOnlyRepository> _mockReadRepository;
    private readonly Mock<IProductWriteOnlyRepository> _mockWriteRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ILogger<ProductService>> _mockLogger;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _mockReadRepository = new Mock<IProductReadOnlyRepository>();
        _mockWriteRepository = new Mock<IProductWriteOnlyRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockLogger = new Mock<ILogger<ProductService>>();

        _productService = new ProductService(
            _mockReadRepository.Object,
            _mockWriteRepository.Object,
            _mockMapper.Object,
            _mockUnitOfWork.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task GetProductById()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Name = "Laptop" };
        var productDto = new ProductDTO { Id = productId, Name = "Laptop" };

        _mockReadRepository.Setup(r => r.GetById(productId)).ReturnsAsync(product);
        _mockMapper.Setup(m => m.Map<ProductDTO>(product)).Returns(productDto);

        // Act
        var result = await _productService.GetProductById(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
    }

    [Fact]
    public async Task GetProducts()
    {
        // Arrange
        var products = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
        var productDtos = new List<ProductDTO> { new ProductDTO { Id = 1 }, new ProductDTO { Id = 2 } };

        _mockReadRepository.Setup(r => r.GetAll()).ReturnsAsync(products);
        _mockMapper.Setup(m => m.Map<IEnumerable<ProductDTO>>(products)).Returns(productDtos);

        // Act
        var result = await _productService.GetProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AddProduct()
    {
        // Arrange
        var productDto = new ProductDTO { Name = "Mouse" };
        var product = new Product { Name = "Mouse" };
        var newProductId = 1;

        _mockMapper.Setup(m => m.Map<Product>(productDto)).Returns(product);
        _mockWriteRepository.Setup(w => w.Create(product)).Callback(() => product.Id = newProductId);

        // Act
        await _productService.AddProduct(productDto);

        // Assert
        _mockWriteRepository.Verify(w => w.Create(product), Times.Once);
        Assert.Equal(newProductId, productDto.Id);
    }

    [Fact]
    public async Task Updateproduct()
    {
        // Arrange
        var productDto = new ProductDTO { Id = 1, Name = "Teclado" };
        var product = new Product { Id = 1, Name = "Teclado" };

        _mockMapper.Setup(m => m.Map<Product>(productDto)).Returns(product);

        // Act
        await _productService.Updateproduct(productDto);

        // Assert
        _mockWriteRepository.Verify(w => w.Update(product), Times.Once);
    }

    [Fact]
    public async Task RemoveProduct()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId };
        _mockReadRepository.Setup(r => r.GetById(productId)).ReturnsAsync(product);

        // Act
        await _productService.RemoveProduct(productId);

        // Assert
        _mockWriteRepository.Verify(w => w.Delete(productId), Times.Once);
    }

}