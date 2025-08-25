using DesafioTecnicoAvanade.VendasApi.DTOs;
using DesafioTecnicoAvanade.VendasApi.DTOs.Request;
using DesafioTecnicoAvanade.VendasApi.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        var order = await _orderService.GetOrderById(id);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost("finalize/{userId}")]
    public async Task<ActionResult<OrderDTO>> FinalizeOrder(string userId)
    {
        var order = await _orderService.FinalizeOrder(userId);
        if (order == null) return BadRequest();
        return Ok(order);
    }
}
