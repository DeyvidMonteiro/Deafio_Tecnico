using DesafioTecnicoAvanade.VendasApi.DTOs;

namespace DesafioTecnicoAvanade.VendasApi.Services.Contracts
{
    public interface IOrderService
    {
        Task<OrderDTO> GetOrderById(int orderId);
        Task<OrderDTO> FinalizeOrder(CartDTO cartDto);
    }
}
