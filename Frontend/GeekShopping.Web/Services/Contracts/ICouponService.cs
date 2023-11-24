using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Contracts;

public interface ICouponService
{
    Task<CouponViewModel> GetCoupon(string couponCode, string token);
}
