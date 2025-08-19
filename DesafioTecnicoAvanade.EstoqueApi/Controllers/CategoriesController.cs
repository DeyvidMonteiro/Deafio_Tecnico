using DesafioTecnicoAvanade.EstoqueApi.DTOs;
using DesafioTecnicoAvanade.EstoqueApi.Roles;
using DesafioTecnicoAvanade.EstoqueApi.Services.Category;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTecnicoAvanade.EstoqueApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryServices _services;

        public CategoriesController(ICategoryServices services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categories = await _services.GetCategories();

            if (categories is null)
                return NotFound();

            return Ok(categories);

        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesProducts()
        {
            var categories = await _services.GetCategoriesProducts();

            if (categories is null)
                return NotFound();

            return Ok(categories);

        }

        [HttpGet("{id:int}", Name = "GetCategory")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var categories = await _services.GetCategoryById(id);

            if (categories is null)
                return NotFound();

            return Ok(categories);

        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> Post([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO is null)
                return BadRequest("Dados Invalidos");

            await _services.AddCategory(categoryDTO);

            return new CreatedAtRouteResult("GetCategory", new { id = categoryDTO.Id }, categoryDTO);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> Put(int id, [FromBody] CategoryDTO categoryDTO)
        {
            if (id != categoryDTO.Id || categoryDTO is null)
                return BadRequest();

            await _services.UpdateCategry(categoryDTO);

            return NoContent();

        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Role.Admin)]
        public async Task<ActionResult> Delete(int id)
        {
         var category = await _services.GetCategoryById(id);

            if (category is null)
                return NotFound();

            await _services.RemoveCategory(id);

            return NoContent();
            
        }

    }
}
