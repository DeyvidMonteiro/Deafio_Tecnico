using DesafioTecnicoAvanade.VendasApi.Models;

namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts
{
    public interface IOrderReadOnlyRepository
    {
        Task<Order> GetOrderByIdAsync(int orderId);
    }
}
