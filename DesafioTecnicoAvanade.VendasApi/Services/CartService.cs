using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.DTOs.Request;
using DesafioTecnicoAvanade.VendasApi.Filters.Exceptions;
using DesafioTecnicoAvanade.VendasApi.Models;
using DesafioTecnicoAvanade.VendasApi.Services.Contracts;
using DesafioTecnicoAvanade.VendasApi.Services.External;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnicoAvanade.VendasApi.Services;

public class CartService : ICartService
{
    private readonly ICartReadOlyRepository _readRepository;
    private readonly ICartWriteOnlyRepository _writeRepository;
    private readonly IMapper _mapper;
    private readonly IProductApiService _productApiService;

    public CartService(ICartReadOlyRepository readeRepository,
        ICartWriteOnlyRepository writeRepository, IMapper mapper,
        IProductApiService productApiService)
    {
        _readRepository = readeRepository;
        _writeRepository = writeRepository;
        _mapper = mapper;
        _productApiService = productApiService;
    }

    public async Task<CartDTO> GetCartByUserId(string userId)
    {
        var cartHeaderWithItems = await _readRepository.GetCartWithItemsByUserIdAsync(userId);

        if (cartHeaderWithItems == null)
            return null;

        var cartDTO = _mapper.Map<CartDTO>(cartHeaderWithItems);

        foreach (var item in cartDTO.CartItems)
        {
            var product = await _productApiService.GetProductByIdAsync(item.ProductId);
            item.Product = _mapper.Map<ProductDTO>(product);
        }

        return cartDTO;
    }

    public async Task<CartDTO> AddCart(string userId, RequestCartDTO requestCartDTO)
    {
        if (requestCartDTO == null)
            throw new ArgumentException("Request não pode ser nulo.");

        var cartHeader = await _readRepository.GetCartHeaderByUserIdAsync(userId);

        if (cartHeader == null)
        {
            cartHeader = new CartHeader { UserId = userId };
            await _writeRepository.AddCartHeaderAsync(cartHeader);
        }

        foreach (var item in requestCartDTO.CartItems)
        {
            if (item.Qauntity <= 0)
                throw new InvalidOrderException("A quantidade do item deve ser maior que zero.");

            var productDTO = await _productApiService.GetProductByIdAsync(item.ProductId);
            if (productDTO == null)
                throw new ProductNotFoundException($"Produto com ID {item.ProductId} não encontrado na API de Estoque.");

            if (productDTO.Stock < item.Qauntity)
                throw new InsufficientStockException($"{item.ProductId} com estoque insuficiente");

            var existingItem = await _readRepository.GetCartItemAsync(item.ProductId, cartHeader.Id);

            if (existingItem != null)
            {
                existingItem.Qauntity += item.Qauntity;
                await _writeRepository.UpdateCartItemAsync(existingItem);
            }
            else
            {
                var newItem = _mapper.Map<CartItem>(item);
                newItem.CartHeaderId = cartHeader.Id;
                await _writeRepository.AddCartItemAsync(newItem);
            }
        }

        return await GetCartByUserId(userId);
    }


    public async Task<bool> CleanCart(string userId)
    {
        var cartHeader = await _readRepository.GetCartHeaderByUserIdAsync(userId);

        if (cartHeader == null) return false;

        var cartItems = _readRepository.GetCartItemsByHeaderId(cartHeader.Id).ToList();

        foreach (var item in cartItems)
            await _writeRepository.DeleteCartItemAsync(item);

        return true;
    }

    public async Task<bool> DeleteItemCart(int cartItemId)
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

}
