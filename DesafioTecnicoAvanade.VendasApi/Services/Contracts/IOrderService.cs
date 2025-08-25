using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.DTOs.Request;

namespace DesafioTecnicoAvanade.VendasApi.Services.Contracts
{
    public interface IOrderService
    {
        Task<OrderDTO> GetOrderById(int orderId);
        Task<OrderDTO> FinalizeOrder(string userId);
    }
}
