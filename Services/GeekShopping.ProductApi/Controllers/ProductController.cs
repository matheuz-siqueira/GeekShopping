using System.Runtime.CompilerServices;
using GeekShopping.ProductApi.Data.ValueObjects;
using GeekShopping.ProductApi.Repositories.contracts;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.ProductApi.Controllers;

[ApiController]
[Route("api/v1/product")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;
    public ProductController(IProductRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository)); 
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductVO>>> GetAll()
    {
        var products = await _repository.GetAll(); 
        if(!products.Any())
        {
            return NoContent(); 
        }
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductVO>> GetById(long id)
    {
        var product = await _repository.GetById(id); 
        if(product is null)
            return NotFound(new {message = "Product not found"});
        
        return Ok(product);
    }   

    [HttpPost]
    public async Task<ActionResult<ProductVO>> Create(ProductVO vo)
    {
        if(vo is null)
        {
            return BadRequest(new {message = "Invalid request"}); 
        }
        await _repository.Create(vo); 
        return StatusCode(201, vo); 
    }

    [HttpPut]
    public async Task<ActionResult<ProductVO>> Update(ProductVO vo)
    {
         if(vo is null)
        {
            return BadRequest(new {message = "Invalid request"}); 
        }
        await _repository.Update(vo); 
        return Ok(vo);  
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(long id)
    {
        var status = await _repository.Delete(id); 
        if(!status)
        {
            return BadRequest(new {message = "Internal error"}); 
        }
        return Ok(status); 
    }
}
