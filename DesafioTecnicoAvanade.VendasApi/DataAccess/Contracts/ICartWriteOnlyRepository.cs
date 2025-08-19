using DesafioTecnicoAvanade.VendasApi.DTOs;

namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts
{
    public interface ICartWriteOnlyRepository
    {
        Task<CartDTO>UpdateCartAsync(CartDTO cartDTO);
        Task<bool> CleanCartAsync(string userId);
        Task<bool> DeleteItemCartAsync(int cartItemId);
        
    }
}
