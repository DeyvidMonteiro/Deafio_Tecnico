using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Models;
using DesafioTecnicoAvanade.VendasApi.Services.Contracts;
using DesafioTecnicoAvanade.VendasApi.Services.External;

namespace DesafioTecnicoAvanade.VendasApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderReadOnlyRepository _readRepository;
        private readonly IOrderWriteOnlyRepository _writeRepository;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly IProductApiService _productApiService;

        public OrderService(IOrderReadOnlyRepository readRepository,
            IOrderWriteOnlyRepository writeRepository, IMapper mapper
            , ICartService cartService, IProductApiService productService)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
            _cartService = cartService;
            _productApiService = productService;
        }

        public async Task<OrderDTO> GetOrderById(int orderId)
        {
            var order = await _readRepository.GetOrderByIdAsync(orderId);
            if (order == null) return null;

            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<OrderDTO> FinalizeOrder(CartDTO cartDto)
        {
            if (cartDto == null || cartDto.CartHeader == null || !cartDto.CartItems.Any())
                throw new InvalidOperationException("Carrinho vazio ou inválido.");

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

        private async Task ValidateAndUpdateStockAsync(CartItemDTO item)
        {
            var product = await _productApiService.GetProductByIdAsync(item.ProductId);

            if (product == null)
                throw new InvalidOperationException($"Produto {item.ProductId} não encontrado.");

            if (product.Stock < item.Qauntity)
                throw new InvalidOperationException($"Produto {product.Name} sem estoque suficiente.");

            long newStock = product.Stock - item.Qauntity;
            await _productApiService.UpdateProductStockAsync(item.ProductId, newStock);
        }

    }
}
