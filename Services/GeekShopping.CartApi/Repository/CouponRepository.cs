using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using GeekShopping.CartApi.Data.ValueObjects;
using GeekShopping.CartApi.Repository.Contracts;

namespace GeekShopping.CartApi.Repository;

public class CouponRepository : ICouponRepository
{
    private readonly HttpClient _client;

    public CouponRepository(HttpClient client)
    {
        _client = client; 
    }
    public async Task<CouponVO> GetCouponByCouponCode(string couponCode, string token)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"/api/v1/coupon/{couponCode}");
        var content = await response.Content.ReadAsStreamAsync(); 
        if(response.StatusCode != HttpStatusCode.OK)
            return new CouponVO(); 
        return JsonSerializer.Deserialize<CouponVO>(content, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
    }
}
