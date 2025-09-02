using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Models;
using DesafioTecnicoAvanade.VendasApi.Services;
using DesafioTecnicoAvanade.VendasApi.Services.Contracts;
using DesafioTecnicoAvanade.VendasApi.Services.External.Contracts;
using Microsoft.Extensions.Logging;
using Moq;

public class OrderServiceTests
{
    private readonly Mock<IOrderReadOnlyRepository> _mockReadRepository;
    private readonly Mock<IOrderWriteOnlyRepository> _mockWriteRepository;
    private readonly Mock<ICartService> _mockCartService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IProductApiService> _mockProductApiService;
    private readonly Mock<ILogger<OrderService>> _mockLogger;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly OrderService _orderService;

    private readonly string _userId = "user1";

    public OrderServiceTests()
    {
        _mockReadRepository = new Mock<IOrderReadOnlyRepository>();
        _mockWriteRepository = new Mock<IOrderWriteOnlyRepository>();
        _mockCartService = new Mock<ICartService>();
        _mockMapper = new Mock<IMapper>();
        _mockProductApiService = new Mock<IProductApiService>();
        _mockLogger = new Mock<ILogger<OrderService>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _orderService = new OrderService(
            _mockReadRepository.Object,
            _mockWriteRepository.Object,
            _mockMapper.Object,
            _mockCartService.Object,
            _mockProductApiService.Object,
            _mockLogger.Object,
            _mockUnitOfWork.Object
        );
    }

    [Fact]
    public async Task GetOrderById()
    {
        // Arrange
        var orderId = 1;
        var order = new Order { Id = orderId, UserId = _userId };
        var orderDto = new OrderDTO { Id = orderId, UserId = _userId };

        _mockReadRepository.Setup(r => r.GetOrderByIdAsync(orderId)).ReturnsAsync(order);
        _mockMapper.Setup(m => m.Map<OrderDTO>(order)).Returns(orderDto);

        // Act
        var result = await _orderService.GetOrderById(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(orderId, result.Id);
    }

    [Fact]
    public async Task FinalizeOrder()
    {
        // Arrange
        var cartDto = new CartDTO
        {
            CartItems = new List<CartItemDTO>
            {
                new CartItemDTO { ProductId = 1, Qauntity = 2, Product = new ProductDTO { Price = 10, Stock = 10 } }
            },
            CartHeader = new CartHeaderDTO { UserId = _userId }
        };
        var order = new Order { Id = 1, UserId = _userId, Total = 20 };
        var orderDto = new OrderDTO { Id = 1, UserId = _userId, Total = 20 };

        _mockCartService.Setup(c => c.GetCartByUserId(_userId)).ReturnsAsync(cartDto);
        _mockMapper.Setup(m => m.Map<Order>(cartDto)).Returns(order);
        _mockWriteRepository.Setup(w => w.AddOrderAsync(order)).ReturnsAsync(order);
        _mockMapper.Setup(m => m.Map<OrderDTO>(order)).Returns(orderDto);
        _mockCartService.Setup(c => c.CleanCart(_userId)).ReturnsAsync(true);
        _mockProductApiService.Setup(p => p.UpdateProductStockAsync(It.IsAny<int>(), It.IsAny<int>())).Returns(Task.CompletedTask);

        // Act
        var result = await _orderService.FinalizeOrder(_userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(20, result.Total);
        _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(), Times.Once);
        _mockProductApiService.Verify(p => p.UpdateProductStockAsync(1, 2), Times.Once);
        _mockCartService.Verify(c => c.CleanCart(_userId), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitTransactionAsync(), Times.Once);
        _mockUnitOfWork.Verify(u => u.RollbackTransactionAsync(), Times.Never);
    }
}