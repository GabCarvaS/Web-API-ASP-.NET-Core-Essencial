using APICatalogo.Context;
using APICatalogo.Filters;
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
        private readonly AppDbContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(AppDbContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiLoggingFilter))]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();         
        }

        [HttpGet("Products")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoryProducts()
        {
            return await _context.Categories.AsNoTracking().Include(p => p.Products).ToListAsync();
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(p => p.CategoryId == id);

            if (category == null)
            {
                return NotFound("Categoria não encontrada...");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Category category)
        {
            if (category is null)
                return BadRequest();

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = category.CategoryId }, category);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest();
            }
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);

            if (category == null)
            {
                return NotFound("Categoria não encontrada...");
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return Ok(category);
        }

    }
}
