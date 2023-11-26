using GeekShopping.CartApi.Data.ValueObjects;
using GeekShopping.CartApi.Messages;
using GeekShopping.CartApi.RabbitMQSender;
using GeekShopping.CartApi.Repository.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace GeekShopping.CartApi.Controllers;

[ApiController]
[Route("api/v1/cart")]
[Produces("application/json")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;
    private readonly ICouponRepository _couponRepository;
    private readonly IRabbitMQMessageSender _rabbitMQMessageSender;
    public CartController(ICartRepository cartRepository, 
        IRabbitMQMessageSender rabbitMQMessageSender, 
        ICouponRepository couponRepository)
    {
        _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
        _couponRepository = couponRepository ?? throw new ArgumentNullException(nameof(couponRepository));
        _rabbitMQMessageSender = rabbitMQMessageSender; 
    }

    [HttpGet("find-cart/{userId}")]
    public async Task<ActionResult<CartVO>> GetById(string userId)
    {
        var cart = await _cartRepository.GetCartByUserId(userId); 
        if(cart is null)
            return NotFound(); 
        return Ok(cart);
    }

    [HttpPost("add-cart")]
    public async Task<ActionResult<CartVO>> AddCart(CartVO cartVO)
    {
        var cart = await _cartRepository.SaveOrUpdateCart(cartVO);
        if(cart is null)
            return BadRequest(); 
        return StatusCode(201, cart); 
    }

    [HttpPut("update-cart")]
    public async Task<ActionResult<CartVO>> UpdateCart(CartVO cartVO)
    {
        var cart = await _cartRepository.SaveOrUpdateCart(cartVO); 
        if(cart is null)
            return BadRequest();
        return Ok(cart); 
    }

    [HttpDelete("remove-cart/{id}")]
    public async Task<ActionResult<bool>> RemoveCart(long id)
    {
        var status = await _cartRepository.RemoveFromCart(id); 
        if(!status)
            return NotFound(); 
        return Ok(status);
    }

    [HttpPost("apply-coupon")]
    public async Task<ActionResult<CartVO>> ApplyCoupon(CartVO cartVO)
    {
        var status = await _cartRepository.ApplyCoupon(
            cartVO.CartHeader.UserId, cartVO.CartHeader.CouponCode); 
        if(!status)
            return NotFound();
        return Ok(status);
    }

    [HttpDelete("remove-coupon/{userId}")]
    public async Task<ActionResult<CartVO>> AddCart(string userId)
    {
        var status = await _cartRepository.RemoveCoupon(userId);
        if(!status)
            return NotFound(); 
        return Ok(status);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO vo)
    {

        // string token = Request.Headers["Authorization"];
        string token = await HttpContext.GetTokenAsync("access_token");

        if(vo?.UserId is null)
            return BadRequest();
            
        var cart = await _cartRepository.GetCartByUserId(vo.UserId);
        if(cart is null)
            return NotFound(); 

        if(!string.IsNullOrEmpty(vo.CouponCode))
        {   
            CouponVO coupon = await _couponRepository
                .GetCouponByCouponCode(vo.CouponCode, token);
            if(vo.DiscountAmount != coupon.DiscountAmount)
            {
                return StatusCode(412);
            } 
        }
        vo.CartDetails = cart.CartDetails; 
        vo.DateTime = DateTime.Now;

        _rabbitMQMessageSender.SendMessage(vo, "checkoutqueue");

        await _cartRepository.ClearCart(vo.UserId); 

        return Ok(vo); 
    }   
}
