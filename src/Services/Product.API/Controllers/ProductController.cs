using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product.API.Entities;
using Product.API.Repositories.Interfaces;
using Shared.Dtos.Product;

namespace Product.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize(IdentityServerAuthenticationDefaults.AuthenticationScheme)]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    #region CRUD
    [HttpGet]
    [Authorize]
   // [ClaimsRequirementAttribute(FunctionCode.PRODUCT, CommandCode.VIEW)]
    public async Task<IActionResult> GetList()
    {
        var products = await _repository.GetProducts();
        var result = _mapper.Map<IEnumerable<ProductDto>>(products);
        return Ok(result);
    }

    [HttpGet("{id}")]
    //[ClaimsRequirementAttribute(FunctionCode.PRODUCT, CommandCode.VIEW)]
    public async Task<IActionResult> Get([Required] int id)
    {
        var product = await _repository.GetProduct(id);
        if (product == null)
            return NotFound();

        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    [HttpPost]
    [Authorize]
   // [ClaimsRequirementAttribute(FunctionCode.PRODUCT, CommandCode.CREATE)]
    public async Task<IActionResult> Create([FromBody] CreateProductDto productDto)
    {
        var product = _mapper.Map<CatalogProduct>(productDto);
        await _repository.CreateProduct(product);
        await _repository.SaveChangesAsync();

        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize]
    // [ClaimsRequirementAttribute(FunctionCode.PRODUCT, CommandCode.UPDATE)]
    public async Task<IActionResult> Update([Required] int id, [FromBody] UpdateProductDto productDto)
    {
        var product = await _repository.GetProduct(id);
        if (product == null)
            return NotFound();

        var updateProduct = _mapper.Map(productDto, product);
        await _repository.UpdateProduct(updateProduct);
        await _repository.SaveChangesAsync();

        var result = _mapper.Map<ProductDto>(updateProduct);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    // [ClaimsRequirementAttribute(FunctionCode.PRODUCT, CommandCode.DELETE)]
    public async Task<IActionResult> Delete([Required] int id)
    {
        var product = await _repository.GetProduct(id);
        if (product == null)
            return NotFound();

        await _repository.DeleteProduct(id);
        await _repository.SaveChangesAsync();

        return NoContent();
    }
    #endregion


    #region Additional Resources
    [HttpGet("productNo")]
   // [ClaimsRequirementAttribute(FunctionCode.PRODUCT, CommandCode.VIEW)]
    public async Task<IActionResult> GetByNo([Required] string productNo)
    {
        var product = await _repository.GetProductByNo(productNo);
        if (product == null)
            return NotFound();

        var result = _mapper.Map<ProductDto>(product);
        return Ok(result);
    }
    #endregion
}