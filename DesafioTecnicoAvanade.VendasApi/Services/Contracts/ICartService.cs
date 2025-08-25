using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.DTOs.Request;
using DesafioTecnicoAvanade.VendasApi.Models;

namespace DesafioTecnicoAvanade.VendasApi.Services.Contracts
{
    public interface ICartService
    {
        Task<CartDTO> GetCartByUserId(string userId);
        Task<CartDTO> AddCart(string userId, RequestCartDTO request);
        Task<bool> CleanCart(string userId);
        Task<bool> DeleteItemCart(int cartItemId);


    }
}
