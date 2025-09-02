using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.DTOs.Request;
using DesafioTecnicoAvanade.VendasApi.Models;
using DesafioTecnicoAvanade.VendasApi.Services;
using DesafioTecnicoAvanade.VendasApi.Services.External.Contracts;
using Moq;

public class CartServiceTests
{
    private readonly Mock<ICartReadOlyRepository> _mockReadRepository;
    private readonly Mock<ICartWriteOnlyRepository> _mockWriteRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IProductApiService> _mockProductApiService;
    private readonly CartService _cartService;

    private readonly string _userId = "user1";
    private readonly ProductDTO _productDto = new ProductDTO { Id = 1, Stock = 10, Price = 100, Name = "Laptop" };

    public CartServiceTests()
    {
        _mockReadRepository = new Mock<ICartReadOlyRepository>();
        _mockWriteRepository = new Mock<ICartWriteOnlyRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockProductApiService = new Mock<IProductApiService>();

        _cartService = new CartService(
            _mockReadRepository.Object,
            _mockWriteRepository.Object,
            _mockMapper.Object,
            _mockProductApiService.Object);
    }

    [Fact]
    public async Task GetCartByUserId()
    {
        var cartHeader = new CartHeader
        {
            Id = 1,
            UserId = _userId,
            CartItems = new List<CartItem>
        {
            new CartItem { ProductId = _productDto.Id, Qauntity = 2 }
        }
        };

        var cartDto = new CartDTO
        {
            CartItems = new List<CartItemDTO>
        {
            new CartItemDTO { ProductId = _productDto.Id, Qauntity = 2 }
        }
        };

        _mockReadRepository.Setup(r => r.GetCartWithItemsByUserIdAsync(_userId))
            .ReturnsAsync(cartHeader);

        _mockMapper.Setup(m => m.Map<CartDTO>(cartHeader))
            .Returns(cartDto);

        _mockProductApiService.Setup(p => p.GetProductByIdAsync(_productDto.Id))
            .ReturnsAsync(_productDto);

        _mockMapper.Setup(m => m.Map<ProductDTO>(_productDto))
            .Returns(_productDto);

        // Act
        var result = await _cartService.GetCartByUserId(_userId);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.CartItems);
        Assert.Equal(_productDto.Id, result.CartItems.First().ProductId);
        Assert.Equal(_productDto.Name, _productDto.Name);
    }

    [Fact]
    public async Task CleanCart()
    {
        // Arrange
        var cartHeader = new CartHeader
        {
            Id = 1,
            UserId = _userId
        };

        var cartItems = new List<CartItem>
        {
            new CartItem { Id = 1, ProductId = 10, Qauntity = 2 },
            new CartItem { Id = 2, ProductId = 20, Qauntity = 1 }
        };

        _mockReadRepository.Setup(r => r.GetCartHeaderByUserIdAsync(_userId))
            .ReturnsAsync(cartHeader);

        _mockReadRepository.Setup(r => r.GetCartItemsByHeaderId(cartHeader.Id))
            .Returns(cartItems.AsQueryable());

        _mockWriteRepository.Setup(w => w.DeleteCartItemAsync(It.IsAny<CartItem>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _cartService.CleanCart(_userId);

        // Assert
        Assert.True(result);

        foreach (var item in cartItems)
        {
            _mockWriteRepository.Verify(w => w.DeleteCartItemAsync(item), Times.Once);
        }
    }

    [Fact]
    public async Task AddCart()
    {
        // Arrange
        var requestCartDto = new RequestCartDTO
        {
            CartItems = new List<RequestCartItemDTO>
            {
                new RequestCartItemDTO { ProductId = _productDto.Id, Qauntity = 1 }
            }
        };

        var cartHeader = new CartHeader { Id = 1, UserId = _userId };
        var cartDto = new CartDTO
        {
            CartItems = new List<CartItemDTO>
            {
                new CartItemDTO { ProductId = _productDto.Id, Qauntity = 1 }
            }
        };

        _mockReadRepository.Setup(r => r.GetCartHeaderByUserIdAsync(_userId))
            .ReturnsAsync((CartHeader)null);

        _mockWriteRepository.Setup(w => w.AddCartHeaderAsync(It.IsAny<CartHeader>()))
            .Callback<CartHeader>(c => c.Id = cartHeader.Id)
            .Returns(Task.CompletedTask);

        _mockProductApiService.Setup(p => p.GetProductByIdAsync(_productDto.Id))
            .ReturnsAsync(_productDto);

        _mockMapper.Setup(m => m.Map<CartItem>(It.IsAny<RequestCartItemDTO>()))
            .Returns(new CartItem { ProductId = _productDto.Id, Qauntity = 1 });

        _mockReadRepository.Setup(r => r.GetCartWithItemsByUserIdAsync(_userId))
            .ReturnsAsync(cartHeader);
        _mockMapper.Setup(m => m.Map<CartDTO>(cartHeader)).Returns(cartDto);

        // Act
        var result = await _cartService.AddCart(_userId, requestCartDto);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.CartItems);
        Assert.Equal(_productDto.Id, result.CartItems.First().ProductId);

        _mockWriteRepository.Verify(w => w.AddCartHeaderAsync(It.IsAny<CartHeader>()), Times.Once);
        _mockWriteRepository.Verify(w => w.AddCartItemAsync(It.IsAny<CartItem>()), Times.Once);
    }
}