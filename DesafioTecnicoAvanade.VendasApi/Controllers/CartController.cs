using AutoMapper;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.DTOs.Request;
using DesafioTecnicoAvanade.VendasApi.Services;
using DesafioTecnicoAvanade.VendasApi.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoAvanade.VendasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _service;
        private readonly IMapper _mapper;

        public CartController(ICartService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("getcart/{userId}")]
        public async Task<ActionResult<CartDTO>> GetByUserId(string userId)
        {
            var cartDto = await _service.GetCartByUserId(userId);

            if (cartDto is null || !cartDto.CartItems.Any())
                return NotFound(new { message = "Não há carrinhos para este usuário." });

            return Ok(cartDto);

        }

        [HttpPost("addcart/{userId}")]
        public async Task<ActionResult<CartDTO>> AddCart(string userId, RequestCartDTO request)
        {
            var cart = await _service.AddCart(userId, request);

            if (cart is null)
                return BadRequest("Não foi possível criar o carrinho.");

            return Ok(cart);
        }

        [HttpDelete("deletecartitem/{id}")]
        public async Task<ActionResult<bool>> DeleteCartItem(int id)
        {
            var status = await _service.DeleteItemCart(id);

            if (!status)
                return BadRequest("Não foi possível deletar o item do carrinho.");

            return NoContent();
        }

        [HttpDelete("ClearCart")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            var result = await _service.CleanCart(userId);
            if (!result)
                return NotFound("Carrinho não encontrado.");


            return Ok("Carrinho limpo com sucesso.");
        }
    }

}

