using APICatalogo.Context;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public ProductsController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("products/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsCategory(int id)
        {
            var products = await _uow.ProductRepository.GetProductsByCategory(id);
            if (products is null)
            {
                return NotFound("Produtos não encontrados...");
            }
            return Ok(products);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _uow.ProductRepository.GetAll();
            if (products is null)
            {
                return NotFound();
            }
            return products.ToList();
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            var product = await _uow.ProductRepository.Get(x => x.ProductId == id);
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

            var newProduct = await _uow.ProductRepository.Create(product);
            await _uow.Commit();

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

            var updated = await _uow.ProductRepository.Update(product);
            await _uow.Commit();

            if (updated is null) 
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
            var product = await _uow.ProductRepository.Get(x => x.ProductId == id);
            if (product is null)
            {
                return NotFound("Produto não encontrado...");
            }

            var deleted = await _uow.ProductRepository.Delete(product);
            await _uow.Commit();

            if (deleted is not null)
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
