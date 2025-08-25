using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Models;

namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts
{
    public interface ICartReadOlyRepository
    {
        Task<CartHeader> GetCartHeaderByUserIdAsync(string userId);
        IQueryable<CartItem> GetCartItemsByHeaderId(int cartHeaderId);
        Task<CartItem> GetCartItemAsync(int productId, int cartHeaderId);
        Task<CartItem> GetCartItemByIdAsync(int cartItemId);
        Task<CartHeader> GetCartHeaderByIdAsync(int cartHeaderId);

    }
}
