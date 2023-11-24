using GeekShopping.CouponApi.Data.ValueObjects;
using GeekShopping.CouponApi.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace GeekShopping.CouponApi.Controllers;

[ApiController]
[Route("api/v1/coupon")]
[Produces("application/json")]
public class CouponController : ControllerBase
{
    private readonly ICouponRepository _repository;
    public CouponController(ICouponRepository repository)
    {
        _repository = repository; 
    }

    [HttpGet("{couponCode}")]
    public async Task<ActionResult<CouponVO>> GetCouponByCoupon(string couponCode)
    {
        var coupon = await _repository.GetCouponByCouponCode(couponCode);
        if(coupon is null) 
            return NotFound();
        return Ok(coupon); 
    }
}
