using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Models;

namespace DesafioTecnicoAvanade.VendasApi.Services.Contracts
{
    public interface ICartService
    {
        Task<CartDTO> GetCartByUserIdAsync(string userId);
        Task<CartDTO> UpdateCartAsync(CartDTO cartDTO);
        Task<bool> CleanCartAsync(string userId);
        Task<bool> DeleteItemCartAsync(int cartItemId);

    }
}
