using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.Models;
using DesafioTecnicoAvanade.VendasApi.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnicoAvanade.VendasApi.Services
{
    public class CartService : ICartService
    {
        private readonly ICartReadOlyRepository _readRepository;
        private readonly ICartWriteOnlyRepository _writeRepository;
        private readonly IMapper _mapper;

        public CartService(ICartReadOlyRepository rideRepository, ICartWriteOnlyRepository writeRepository, IMapper mapper)
        {
            _readRepository = rideRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
        }

        public async Task<CartDTO> GetCartByUserIdAsync(string userId)
        {
            var cartHeader = await _readRepository.GetCartHeaderByUserIdAsync(userId);

            if (cartHeader == null)
                return null;

            var cartItems = _readRepository.GetCartItemsByHeaderId(cartHeader.Id);

            var cart = new Cart
            {
                CartHeader = cartHeader,
                CartItems = cartItems
            };

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<CartDTO> UpdateCartAsync(CartDTO cartDTO)
        {
            var cart = _mapper.Map<Cart>(cartDTO);

            await SaveProductInDatabase(cartDTO, cart);

            var cartHeader = await _readRepository.GetCartHeaderByUserIdAsync(cart.CartHeader.UserId);

            if (cartHeader == null)
            {
                await CreateCartHeaderAndItems(cart);
            }
            else
            {
                await UpdateQuantityAndItems(cartDTO, cart, cartHeader);
            }

            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<bool> CleanCartAsync(string userId)
        {
            var cartHeader = await _readRepository.GetCartHeaderByUserIdAsync(userId);

            if (cartHeader == null) return false;

            var cartItems = _readRepository.GetCartItemsByHeaderId(cartHeader.Id).ToList();

            foreach (var item in cartItems)
                await _writeRepository.DeleteCartItemAsync(item);

            await _writeRepository.DeleteCartHeaderAsync(cartHeader);

            return true;
        }

        public async Task<bool> DeleteItemCartAsync(int cartItemId)
        {
            var cartItem = await _readRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem == null) return false;

            await _writeRepository.DeleteCartItemAsync(cartItem);

            var remainingItems = await _readRepository.GetCartItemsByHeaderId(cartItem.CartHeaderId).CountAsync();

            if (remainingItems == 0)
            {
                var cartHeader = await _readRepository.GetCartHeaderByIdAsync(cartItem.CartHeaderId);
                if (cartHeader != null)
                {
                    await _writeRepository.DeleteCartHeaderAsync(cartHeader);
                }
            }

            return true;
        }

        private async Task CreateCartHeaderAndItems(Cart cart)
        {
            await _writeRepository.AddCartHeaderAsync(cart.CartHeader);

            var item = cart.CartItems.First();
            item.CartHeaderId = cart.CartHeader.Id;
            item.Product = null;

            await _writeRepository.AddCartItemAsync(item);
        }

        private async Task SaveProductInDatabase(CartDTO cartDTO, Cart cart)
        {
            var product = await _readRepository.GetProductByIdAsync(cartDTO.CartItems.First().ProductId);
            if (product == null)
            {
                await _writeRepository.AddProductAsync(cart.CartItems.First().Product);
            }
        }
        private async Task UpdateQuantityAndItems(CartDTO cartDTO, Cart cart, CartHeader cartHeader)
        {
            var cartDetail = await _readRepository.GetCartItemAsync(cartDTO.CartItems.First().ProductId, cartHeader.Id);

            var item = cart.CartItems.First();
            item.Product = null;

            if (cartDetail == null)
            {
                item.CartHeaderId = cartHeader.Id;
                await _writeRepository.AddCartItemAsync(item);
            }
            else
            {
                item.Id = cartDetail.Id;
                item.CartHeaderId = cartDetail.CartHeaderId;
                item.Qauntity += cartDetail.Qauntity;
                await _writeRepository.UpdateCartItemAsync(item);
            }

        }
    }
}
