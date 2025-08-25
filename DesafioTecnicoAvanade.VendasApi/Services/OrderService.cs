using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Models;
using DesafioTecnicoAvanade.VendasApi.Services.Contracts;
using DesafioTecnicoAvanade.VendasApi.Services.External;

namespace DesafioTecnicoAvanade.VendasApi.Services;

public class OrderService : IOrderService
{
    private readonly IOrderReadOnlyRepository _readRepository;
    private readonly IOrderWriteOnlyRepository _writeRepository;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;
    private readonly IProductApiService _productApiService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IOrderReadOnlyRepository readRepository,
        IOrderWriteOnlyRepository writeRepository, IMapper mapper
        , ICartService cartService, IProductApiService productService, ILogger<OrderService> logger)
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _mapper = mapper;
        _cartService = cartService;
        _productApiService = productService;
        _logger = logger;
    }

    public async Task<OrderDTO> GetOrderById(int orderId)
    {
        var order = await _readRepository.GetOrderByIdAsync(orderId);
        if (order == null) return null;

        return _mapper.Map<OrderDTO>(order);
    }

    public async Task<OrderDTO> FinalizeOrder(string userId)
    {
        var cartDto = await _cartService.GetCartByUserId(userId);

        if (cartDto == null || cartDto.CartItems.Count == 0)
            throw new InvalidOperationException("Carrinho não encontrado ou vazio.");

        try
        {
            foreach (var item in cartDto.CartItems)
                await ValidateAndUpdateStockAsync(item);

            var order = _mapper.Map<Order>(cartDto);
            order.UserId = cartDto.CartHeader.UserId;
            order.OrderDate = DateTime.UtcNow;

            order.Total = cartDto.CartItems.Sum(i => i.Product.Price * i.Qauntity);

            var createdOrder = await _writeRepository.AddOrderAsync(order);

            await _cartService.CleanCart(order.UserId);

            return _mapper.Map<OrderDTO>(createdOrder);
        }
        catch (Exception ex)
        {
            await RevertStockChangesAsync(cartDto.CartItems);

            throw new InvalidOperationException("Falha ao finalizar o pedido. O estoque foi revertido.", ex);
        }
    }

    private async Task ValidateAndUpdateStockAsync(CartItemDTO item)
    {
        var product = await _productApiService.GetProductByIdAsync(item.ProductId);

        if (product == null)
            throw new InvalidOperationException($"Produto com ID {item.ProductId} não encontrado na API de Estoque.");

        await _productApiService.UpdateProductStockAsync(item.ProductId, item.Qauntity);
    }

    private async Task RevertStockChangesAsync(IEnumerable<CartItemDTO> cartItems)
    {
        foreach (var item in cartItems)
        {
            try
            {
                var product = await _productApiService.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    long revertedStock = product.Stock + item.Qauntity;
                    await _productApiService.UpdateProductStockAsync(item.ProductId, revertedStock);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha crítica ao reverter o estoque para o produto {ProductId}.", item.ProductId);
            }
        }
    }

}
