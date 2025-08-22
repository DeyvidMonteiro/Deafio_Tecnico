using DesafioTecnicoAvanade.VendasApi.Models;

namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts
{
    public interface IOrderWriteOnlyRepository
    {
        Task<Order> AddOrderAsync(Order order);
    }
}
