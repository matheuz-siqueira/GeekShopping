using GeekShopping.CouponApi.Data.ValueObjects;

namespace GeekShopping.CouponApi.Repository.Contracts;

public interface ICouponRepository
{
    Task<CouponVO> GetCouponByCouponCode(string couponCode);
}
