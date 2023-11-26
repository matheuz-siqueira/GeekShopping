using GeekShopping.CartApi.Data.ValueObjects;

namespace GeekShopping.CartApi.Repository.Contracts;

public interface ICouponRepository
{
    Task<CouponVO> GetCouponByCouponCode(string couponCode, string token);
}
