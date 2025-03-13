using APICatalogo.DTO_s;
using APICatalogo.DTO_s.Mappings;
using APICatalogo.Filters;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categories = await _uow.CategoryRepository.GetAll();
            if (categories is null) { return NotFound("Não existem categorias..."); }

            var categoriesDTO = categories.ToCategoryDTOList();

            return Ok(categoriesDTO);      
        }

        [HttpGet("Pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get([FromQuery] CategoriesParameters categoriesParameters)
        {
            var categories = await _uow.CategoryRepository.GetCategories(categoriesParameters);

            return GetCategories(categories);
        }

        [HttpGet("filter/name/pagination")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategoriesFiltredByName([FromQuery] CategoriesNameFilter categoriesParameters)
        {
            var categories = await _uow.CategoryRepository.GetCategoriesFiltredByName(categoriesParameters);

            return GetCategories(categories);
        }

        private ActionResult<IEnumerable<CategoryDTO>> GetCategories(PagedList<Category> categories)
        {
            var metadata = new
            {
                categories.TotalCount,
                categories.PageSize,
                categories.CurrentPage,
                categories.TotalPages,
                categories.HasNext,
                categories.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var categoriesDTO = categories.ToCategoryDTOList();

            return Ok(categoriesDTO);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var category = await _uow.CategoryRepository.Get(x => x.CategoryId == id);

            if (category is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrada...");
                return NotFound($"Categoria com id = {id} não encontrada...");
            }

            var categoryDto = category.ToCategoryDTO();

            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO categoryDto)
        {
            if (categoryDto is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            var category = categoryDto.ToCategory();
               
            var newCategory = await _uow.CategoryRepository.Create(category);
            await _uow.Commit();

            var newCategoryDTO = newCategory.ToCategoryDTO();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = newCategoryDTO.CategoryId }, newCategoryDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Put(int id, CategoryDTO categoryDto)
        {
            if (categoryDto is null)
            {
                _logger.LogWarning($"Dados inválidos...");
                return BadRequest("Dados inválidos...");
            }

            var category = categoryDto.ToCategory();

            var updatedCategory = await _uow.CategoryRepository.Update(category);
            await _uow.Commit();

            var updatedCategoryDTO = updatedCategory.ToCategoryDTO();

            return Ok(updatedCategoryDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Delete(int id)
        {
            var category = await _uow.CategoryRepository.Get(x => x.CategoryId == id);

            if (category is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrada...");
                return NotFound($"Categoria com id = {id} não encontrada...");
            }

            var removedCategory = await _uow.CategoryRepository.Delete(category);
            await _uow.Commit();

            var removedCategoryDTO = removedCategory.ToCategoryDTO();

            return Ok(removedCategoryDTO);
        }

    }
}
