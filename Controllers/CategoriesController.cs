using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IUnitOfWork uow, ILogger<CategoriesController> logger)
        {   
            _uow = uow;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            var categories = await _uow.CategoryRepository.GetAll();
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
            var category = await _uow.CategoryRepository.Get(x => x.CategoryId == id);

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
               
            var newCategory = await _uow.CategoryRepository.Create(category);
            await _uow.Commit();

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

            var updatedCategory = await _uow.CategoryRepository.Update(category);
            await _uow.Commit();
            return Ok(updatedCategory);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _uow.CategoryRepository.Get(x => x.CategoryId == id);

            if (category is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrada...");
                return NotFound($"Categoria com id = {id} não encontrada...");
            }

            var removedCategory = await _uow.CategoryRepository.Delete(category);
            await _uow.Commit();

            return Ok(removedCategory);
        }

    }
}
