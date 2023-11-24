using System.Net;
using System.Net.Http.Headers;
using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Contracts;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services;

public class CouponService : ICouponService
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/coupon"; 
    public CouponService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }
    public async Task<CouponViewModel> GetCoupon(string couponCode, string token)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token); 

        var response = await _client.GetAsync($"{BasePath}/{couponCode}"); 
        if(response.StatusCode != HttpStatusCode.OK)
            return new CouponViewModel(); 
        return await response.ReadContentAs<CouponViewModel>(); 
    }
}
