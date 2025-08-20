 using DesafioTecnicoAvanade.VendasApi.DataAccess.Contracts;
using DesafioTecnicoAvanade.VendasApi.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoAvanade.VendasApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartReadOlyRepository _ReadRepository;
        private readonly ICartWriteOnlyRepository _WriteRepository;

        public CartController(ICartReadOlyRepository cartReadOlyRepository,
            ICartWriteOnlyRepository cartWriteOnlyRepository)
        {
            _ReadRepository = cartReadOlyRepository;
            _WriteRepository = cartWriteOnlyRepository;
        }

        [HttpGet("getcart/{id}")]
        public async Task<ActionResult<CartDTO>> GetByUserId(string userId)
        {
            var cartDto = await _ReadRepository.GetCartByUserIdAsync(userId);

            if (cartDto is null)
                return NotFound();

            return Ok(cartDto);

        }

        [HttpPost("addcart")]
        public async Task<ActionResult<CartDTO>> AddCart(CartDTO cartDTO)
        {
            var cart = await _WriteRepository.UpdateCartAsync(cartDTO);

            if (cart is null)
                return NotFound();

            return Ok(cart);
        }

        [HttpPost("updatecart")]
        public async Task<ActionResult<CartDTO>> UpdateCart(CartDTO cartDTO)
        {
            var cart = await _WriteRepository.UpdateCartAsync(cartDTO);

            if (cart is null)
                return NotFound();

            return Ok(cart);
        }

        [HttpDelete("deletecart/{id}")]
        public async Task<ActionResult<bool>> Deletecart(int id)
        {
            var status = await _WriteRepository.DeleteItemCartAsync(id);

            if(!status)
                return BadRequest();

            return Ok(status);
        }


    }

}
