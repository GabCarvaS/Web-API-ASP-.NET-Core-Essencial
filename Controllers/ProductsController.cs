using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductsController(IProductRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _repository.GetProducts().ToListAsync();
            if (products is null)
            {
                return NotFound();
            }
            return products;
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _repository.GetProduct(id);
            if (product is null)
            {
                return NotFound("Produto não encontrado...");
            }
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Product product)
        {
            if (product is null)
                return BadRequest();

            var newProduct = await _repository.Create(product);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = newProduct.ProductId }, newProduct);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            var updated = await _repository.Update(product);

            if (updated) 
            { 
                return Ok(product); 
            }
            else
            {
                return StatusCode(500, $"Falha ao atualizar o produto de id = {id}");
            }           
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var deleted = await _repository.Delete(id);

            if (deleted)
            {
                return Ok($"Produto de id = {id} foi removido");
            }
            else
            {
                return StatusCode(500, $"Falha ao remover o produto de id = {id}");
            }
        }
    }
}
