using DesafioTecnicoAvanade.EstoqueApi.DTOs;
using DesafioTecnicoAvanade.EstoqueApi.Roles;
using DesafioTecnicoAvanade.EstoqueApi.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoAvanade.EstoqueApi.Controllers;

[Route("[controller]")]
[ApiController]

public class ProductsController : ControllerBase
{
    private readonly IProductService _services;

    public ProductsController(IProductService service)
    {
        _services = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
    {
        var products = await _services.GetProducts();

        if (products is null)
            return NotFound();

        return Ok(products);

    }
   
    [HttpGet("{id:int}", Name = "GetProducts")]
    [Authorize]
    public async Task<ActionResult<CategoryDTO>> Get(int id)
    {
        var products = await _services.GetProductById(id);

        if (products is null)
            return NotFound();

        return Ok(products);

    }

    [HttpPost]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult> Post([FromBody] ProductDTO productDTO)
    {
        if (productDTO is null)
            return BadRequest("Dados Invalidos");

        await _services.AddProduct(productDTO);

        return new CreatedAtRouteResult("GetProducts", new { id = productDTO.Id }, productDTO);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult> Put(int id, [FromBody] ProductDTO productDTO)
    {
        if (id != productDTO.Id || productDTO is null)
            return BadRequest();

        await _services.Updateproduct(productDTO);

        return NoContent();

    }

    [HttpPut("{id}/decrement")]
    [Authorize]
    public async Task<ActionResult> DecrementStock(int id, [FromBody] int quantity)
    {
        await _services.DecrementStock(id, quantity);
        return NoContent();

    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Role.Admin)]
    public async Task<ActionResult> Delete(int id)
    {
        var product = await _services.GetProductById(id);

        if (product is null)
            return NotFound();

        await _services.RemoveProduct(id);

        return NoContent();

    }

}
