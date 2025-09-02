using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Filters.Exceptions;
using DesafioTecnicoAvanade.VendasApi.Models;
using DesafioTecnicoAvanade.VendasApi.Services.Contracts;
using DesafioTecnicoAvanade.VendasApi.Services.External.Contracts;

namespace DesafioTecnicoAvanade.VendasApi.Services;

public class OrderService : IOrderService
{
    private readonly IOrderReadOnlyRepository _readRepository;
    private readonly IOrderWriteOnlyRepository _writeRepository;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;
    private readonly IProductApiService _productApiService;
    private readonly ILogger<OrderService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IOrderReadOnlyRepository readRepository,
        IOrderWriteOnlyRepository writeRepository, IMapper mapper
        , ICartService cartService, IProductApiService productService, ILogger<OrderService> logger, IUnitOfWork unitOfWork)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _mapper = mapper;
        _cartService = cartService;
        _productApiService = productService;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<OrderDTO> GetOrderById(int orderId)
    {
        var order = await _readRepository.GetOrderByIdAsync(orderId);
        if (order == null) return null;

        return _mapper.Map<OrderDTO>(order);
    }

    public async Task<OrderDTO> FinalizeOrder(string userId)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var cartDto = await _cartService.GetCartByUserId(userId);

            if (cartDto == null || cartDto.CartItems.Count == 0)
            {
                throw new InvalidOrderException("Carrinho não encontrado ou vazio.");
            }

            foreach (var item in cartDto.CartItems)
            {
                await _productApiService.UpdateProductStockAsync(item.ProductId, item.Qauntity);
            }

            var order = _mapper.Map<Order>(cartDto);
            order.UserId = cartDto.CartHeader.UserId;
            order.OrderDate = DateTime.UtcNow;
            order.Total = cartDto.CartItems.Sum(i => i.Product.Price * i.Qauntity);

            var createdOrder = await _writeRepository.AddOrderAsync(order);

            await _cartService.CleanCart(order.UserId);

            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<OrderDTO>(createdOrder);
        }
        catch (Exception)
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

}
