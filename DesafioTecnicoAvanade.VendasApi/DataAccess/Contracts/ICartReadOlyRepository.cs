using DesafioTecnicoAvanade.VendasApi.DTOs;

namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts
{
    public interface ICartReadOlyRepository
    {
        Task<CartDTO> GetCartByUserIdAsync(string userId);
    }
}
