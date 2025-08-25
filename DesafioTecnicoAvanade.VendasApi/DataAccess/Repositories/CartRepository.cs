using DesafioTecnicoAvanade.VendasApi.DataAccess.Context;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Repositories
{
    public class CartRepository : ICartReadOlyRepository, ICartWriteOnlyRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IUnitOfWork _unitOfWork;

        public CartRepository(AppDbContext appDbContext, IUnitOfWork unitOfWork)
        {
            _appDbContext = appDbContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartHeader> GetCartHeaderByUserIdAsync(string userId)
        {
            return await _appDbContext.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CartHeader> GetCartHeaderByIdAsync(int cartHeaderId)
        {
            return await _appDbContext.CartHeaders.FirstOrDefaultAsync(c => c.Id == cartHeaderId);
        }

        public IQueryable<CartItem> GetCartItemsByHeaderId(int cartHeaderId)//
        {
            return _appDbContext.CartItems.Where(c => c.CartHeaderId == cartHeaderId);
        }

        public async Task<CartItem> GetCartItemAsync(int productId, int cartHeaderId)
        {
            return await _appDbContext.CartItems.FirstOrDefaultAsync(c => c.ProductId == productId && c.CartHeaderId == cartHeaderId);
        }

        public async Task<CartItem> GetCartItemByIdAsync(int cartItemId)
        {
            return await _appDbContext.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);
        }

        public async Task AddCartHeaderAsync(CartHeader cartHeader)
        {
            _appDbContext.CartHeaders.Add(cartHeader);
            await _unitOfWork.Commit();
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            _appDbContext.CartItems.Add(cartItem);
            await _unitOfWork.Commit();
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _appDbContext.CartItems.Update(cartItem);
            await _unitOfWork.Commit();
        }

        public async Task DeleteCartItemAsync(CartItem cartItem)
        {
            _appDbContext.CartItems.Remove(cartItem);
            await _unitOfWork.Commit();
        }

        public async Task DeleteCartHeaderAsync(CartHeader cartHeader)
        {
            _appDbContext.CartHeaders.Remove(cartHeader);
            await _unitOfWork.Commit();
        }

    }

}