using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryRepository repository, ILogger<CategoriesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            var categories = await _repository.GetCategories();
            return Ok(categories);      
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoryProducts()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var category = await _repository.GetCategory(id);

            if (category is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrada...");
                return NotFound($"Categoria com id = {id} não encontrada...");
            }

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Category category)
        {
            if (category is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }
               
            var newCategory = await _repository.Create(category);

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = newCategory.CategoryId }, newCategory);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Category category)
        {
            if (category is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            var updatedCategory = await _repository.Update(category);            
            return Ok(updatedCategory);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _repository.GetCategory(id);

            if (category is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrada...");
                return NotFound($"Categoria com id = {id} não encontrada...");
            }

            var removedCategory = await _repository.Delete(id);
            return Ok(removedCategory);
        }

    }
}
