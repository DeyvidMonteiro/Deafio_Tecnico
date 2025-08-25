using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Models;

namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts
{
    public interface ICartWriteOnlyRepository
    {
        Task AddCartHeaderAsync(CartHeader cartHeader);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(CartItem cartItem);
        Task DeleteCartHeaderAsync(CartHeader cartHeader);

    }
}
