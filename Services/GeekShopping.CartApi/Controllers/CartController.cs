using GeekShopping.CartApi.Data.ValueObjects;
using GeekShopping.CartApi.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace GeekShopping.CartApi.Controllers;

[ApiController]
[Route("api/v1/cart")]
[Produces("application/json")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _repository;
    public CartController(ICartRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("find-cart/{userId}")]
    public async Task<ActionResult<CartVO>> GetById(string userId)
    {
        var cart = await _repository.GetCartByUserId(userId); 
        if(cart is null)
            return NotFound(); 
        return Ok(cart);
    }

    [HttpPost("add-cart")]
    public async Task<ActionResult<CartVO>> AddCart(CartVO cartVO)
    {
        var cart = await _repository.SaveOrUpdateCart(cartVO);
        if(cart is null)
            return BadRequest(); 
        return StatusCode(201, cart); 
    }

    [HttpPut("update-cart")]
    public async Task<ActionResult<CartVO>> UpdateCart(CartVO cartVO)
    {
        var cart = await _repository.SaveOrUpdateCart(cartVO); 
        if(cart is null)
            return BadRequest();
        return Ok(cart); 
    }

    [HttpDelete("remove-cart/{id}")]
    public async Task<ActionResult<bool>> RemoveCart(long id)
    {
        var status = await _repository.RemoveFromCart(id); 
        if(!status)
            return NotFound(); 
        return Ok(status);
    }
}
