using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Models;
using DesafioTecnicoAvanade.VendasApi.Services.Contracts;

namespace DesafioTecnicoAvanade.VendasApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderReadOnlyRepository _readRepository;
        private readonly IOrderWriteOnlyRepository _writeRepository;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public OrderService(IOrderReadOnlyRepository readRepository,
            IOrderWriteOnlyRepository writeRepository, IMapper mapper, ICartService cartService)
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
            _cartService = cartService;
        }

        public async Task<OrderDTO> GetOrderById(int orderId)
        {
            var order = await _readRepository.GetOrderByIdAsync(orderId);
            if (order == null) return null;

            return _mapper.Map<OrderDTO>(order);
        }


        public async Task<OrderDTO> FinalizeOrder(CartDTO cartDto)
        {
            var order = _mapper.Map<Order>(cartDto);

            order.UserId = cartDto.CartHeader.UserId;
            order.OrderDate = DateTime.UtcNow;
            order.Total = cartDto.CartItems.Sum(i => i.Product.Price * i.Qauntity);

            var createdOrder = await _writeRepository.AddOrderAsync(order);

            await _cartService.CleanCartAsync(order.UserId);

            return _mapper.Map<OrderDTO>(createdOrder);
        }

    }
}
