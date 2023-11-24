using AutoMapper;
using GeekShopping.CouponApi.Data.ValueObjects;
using GeekShopping.CouponApi.Model.Context;
using GeekShopping.CouponApi.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CouponApi.Repository;

public class CouponRepository : ICouponRepository
{
    private readonly AppDbContext _context; 
    private readonly IMapper _mapper;
    public CouponRepository(AppDbContext context, IMapper mapper)
    {
        _context = context; 
        _mapper = mapper; 
    }
    public async Task<CouponVO> GetCouponByCouponCode(string couponCode)
    {
        var coupon = await _context.Coupons
            .FirstOrDefaultAsync(c => c.CouponCode == couponCode);
        return _mapper.Map<CouponVO>(coupon); 
    }
}
