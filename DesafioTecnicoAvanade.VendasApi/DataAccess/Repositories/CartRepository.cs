using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Context;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;

namespace DesafioTecnicoAvanade.VendasApi.DataAccess.Repositories
{
    public class CartRepository : ICartReadOlyRepository, ICartWriteOnlyRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartRepository(AppDbContext appDbContext, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {

            Cart cart = new()
            {
                CartHeader = await _appDbContext.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId),
            };

            if (cart.CartHeader == null)
                return null;

            cart.CartItems = _appDbContext.CartItems.Where(c => c.CartHeaderId == cart.CartHeader.Id).Include(c => c.Product);

            return _mapper.Map<CartDTO>(cart);

        }

        public async Task<bool> DeleteItemCartAsync(int cartItemId)
        {

            try
            {
                CartItem cartItem = await _appDbContext.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId);

                int total = _appDbContext.CartItems.Where(c => c.CartHeaderId == cartItem.CartHeaderId).Count();

                _appDbContext.CartItems.Remove(cartItem);
                await _unitOfWork.Commit();

                if (total == 1)
                {
                    var cartHeaderRemove = await _appDbContext.CartHeaders.FirstOrDefaultAsync(c => c.Id == cartItem.CartHeaderId);
                    _appDbContext.CartHeaders.Remove(cartHeaderRemove);
                    await _unitOfWork.Commit();
                }


                return true;

            }
            catch (Exception)
            {
                return false;
            }

        }

        public async Task<bool> CleanCartAsync(string userId)
        {
            var cartHeader = await _appDbContext.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

            if (cartHeader is not null)
            {
                _appDbContext.CartItems.RemoveRange(_appDbContext.CartItems.Where(c => c.CartHeaderId == cartHeader.Id));

                _appDbContext.CartHeaders.Remove(cartHeader);

                await _unitOfWork.Commit();
                return true;
            }

            return false;
        }

        public async Task<CartDTO> UpdateCartAsync(CartDTO cartDTO)
        {
            //salva se nao existir
            Cart cart = _mapper.Map<Cart>(cartDTO);

            await SaveproductInDataBase(cartDTO, cart);

            //verifica se cartheader é null
            var cartHeader = await _appDbContext.CartHeaders.AsNoTracking()
                .FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

            if (cartHeader is null)
            {

                await CreateCartHeaderAndItems(cart);
            }
            else
            {
                await UpdateQuantityAndItems(cartDTO, cart, cartHeader);
            }

            return _mapper.Map<CartDTO>(cart);
        }

        private async Task UpdateQuantityAndItems(CartDTO cartDTO, Cart cart, CartHeader? cartHeader)
        {

            var cartDetail = await _appDbContext.CartItems.AsNoTracking().FirstOrDefaultAsync(
                                   p => p.ProductId == cartDTO.CartItems.FirstOrDefault()
                                   .ProductId && p.CartHeaderId == cartHeader.Id);

            if (cartDetail is null)
            {
                //Cria o CartItems
                cart.CartItems.FirstOrDefault().CartHeaderId = cartHeader.Id;
                cart.CartItems.FirstOrDefault().Product = null;
                _appDbContext.CartItems.Add(cart.CartItems.FirstOrDefault());
            }
            else
            {
                //Atualiza a quantidade e o CartItems
                cart.CartItems.FirstOrDefault().Product = null;
                cart.CartItems.FirstOrDefault().Qauntity += cartDetail.Qauntity;
                cart.CartItems.FirstOrDefault().Id = cartDetail.Id;
                cart.CartItems.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId;
                _appDbContext.CartItems.Update(cart.CartItems.FirstOrDefault());
            }
            await _unitOfWork.Commit();
        }

        private async Task CreateCartHeaderAndItems(Cart cart)
        {
            _appDbContext.CartHeaders.Add(cart.CartHeader);
            await _unitOfWork.Commit();

            cart.CartItems.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
            cart.CartItems.FirstOrDefault().Product = null;

            _appDbContext.CartItems.Add(cart.CartItems.FirstOrDefault());

            await _unitOfWork.Commit();
        }

        private async Task SaveproductInDataBase(CartDTO cartDTO, Cart cart)
        {
            //verifica se ja foi salvo
            var product = await _appDbContext.Products
                .FirstOrDefaultAsync(p => p.Id == cartDTO.CartItems.FirstOrDefault().ProductId);

            if (product is null)
            {
                _appDbContext.Products.Add(cart.CartItems.FirstOrDefault().Product);
                await _unitOfWork.Commit();
            }
        }
    }
}
