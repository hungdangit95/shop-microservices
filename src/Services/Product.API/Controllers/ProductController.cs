using Contracts.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IRepositoryBaseAsync<CatalogProduct, long, ProductContext> _productRepository;
        public ProductController(IRepositoryBaseAsync<CatalogProduct, long, ProductContext> productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productRepository.FindAll().ToListAsync());
        }
    }
}
