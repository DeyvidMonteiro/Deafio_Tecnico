using DesafioTecnicoAvanade.VendasApi.DTOs;
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

        public CartController(ICartService service)
        {
            _service = service;
        }

        [HttpGet("getcart/{userId}")]
        public async Task<ActionResult<CartDTO>> GetByUserId(string userId)
        {
            var cartDto = await _service.GetCartByUserIdAsync(userId);

            if (cartDto is null)
                return NotFound();

            return Ok(cartDto);

        }

        [HttpPost("addcart")]
        public async Task<ActionResult<CartDTO>> AddCart(CartDTO cartDTO)
        {
            var cart = await _service.UpdateCartAsync(cartDTO);

            if (cart is null)
                return BadRequest("Não foi possível criar o carrinho.");

            return Ok(cart);
        }

        [HttpPost("updatecart")]
        public async Task<ActionResult<CartDTO>> UpdateCart(CartDTO cartDTO)
        {
            var cart = await _service.UpdateCartAsync(cartDTO);

            if (cart is null)
                return BadRequest();

            return Ok(cart);
        }

        [HttpDelete("deletecart/{id}")]
        public async Task<ActionResult<bool>> DeleteCart(int id)
        {
            var status = await _service.DeleteItemCartAsync(id);

            if (!status)
                return BadRequest("Não foi possível deletar o item do carrinho.");

            return Ok(status);
        }
    }

}

