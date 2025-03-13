using APICatalogo.Context;
using APICatalogo.DTO_s;
using APICatalogo.Interfaces;
using APICatalogo.Models;
using APICatalogo.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        [HttpGet("products/{id}")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsCategory(int id)
        {
            var products = await _uow.ProductRepository.GetProductsByCategory(id);

            if (products is null)
            {
                return NotFound("Produtos não encontrados...");
            }

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDTO);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get()
        {
            var products = await _uow.ProductRepository.GetAll();

            if (products is null)
            {
                return NotFound();
            }

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productsDTO);
        }

        [HttpGet("Pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] ProductsParameters productsParameters)
        {
            var products = await _uow.ProductRepository.GetProducts(productsParameters);

            var metadata = new 
            {
                products.TotalCount,
                products.PageSize,
                products.CurrentPage,
                products.TotalPages,
                products.HasNext,
                products.HasPrevious
            };

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

            var productsDTO = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return Ok(productsDTO);
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<ProductDTO>> Get(int id)
        {
            var product = await _uow.ProductRepository.Get(x => x.ProductId == id);

            if (product is null)
            {
                return NotFound("Produto não encontrado...");
            }

            var productDTO = _mapper.Map<ProductDTO>(product);

            return Ok(productDTO);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post(ProductDTO productDTO)
        {
            if (productDTO is null)
                return BadRequest();


            var product = _mapper.Map<Product>(productDTO);

            var newProduct = await _uow.ProductRepository.Create(product);
            await _uow.Commit();

            var newProductDTO = _mapper.Map<ProductDTO>(newProduct);

            return new CreatedAtRouteResult("ObterProduto",
                new { id = newProductDTO.ProductId }, newProductDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Put(int id, ProductDTO productDTO)
        {
            if (id != productDTO.ProductId)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productDTO);

            var updatedProduct = await _uow.ProductRepository.Update(product);
            await _uow.Commit();

            var updatedProductdDTO = _mapper.Map<ProductDTO>(updatedProduct);

            if (updatedProductdDTO is null) 
            { 
                return Ok(updatedProductdDTO); 
            }
            else
            {
                return StatusCode(500, $"Falha ao atualizar o produto de id = {id}");
            }           
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProductDTO>> Delete(int id)
        {
            var product = await _uow.ProductRepository.Get(x => x.ProductId == id);
            if (product is null)
            {
                return NotFound("Produto não encontrado...");
            }

            var deletedProduct = await _uow.ProductRepository.Delete(product);
            await _uow.Commit();

            var deletedProductdDTO = _mapper.Map<ProductDTO>(deletedProduct);

            if (deletedProductdDTO is not null)
            {
                return Ok(deletedProductdDTO);
            }
            else
            {
                return StatusCode(500, $"Falha ao remover o produto de id = {id}");
            }
        }
    }
}
