using System.Net.Http.Headers;
using GeekShopping.Web.Models;
using GeekShopping.Web.Services.Contracts;
using GeekShopping.Web.Utils;

namespace GeekShopping.Web.Services;

public class CartService : ICartService
{
    private readonly HttpClient _client; 
    public const string BasePath = "api/v1/cart";

    public CartService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task<CartViewModel> GetCartByUserId(string userId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"{BasePath}/find-cart/{userId}");
        return await response.ReadContentAs<CartViewModel>();
    }
    public async Task<CartViewModel> AddItemToCart(CartViewModel model, string token)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token); 
        var response = await _client.PostAsJons($"{BasePath}/add-cart", model);
        if(response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>(); 
        else 
            throw new Exception("Something went wrong when calling API");
    }
    public async Task<CartViewModel> UpdateCart(CartViewModel model, string token)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token); 
        var response = await _client.PuttAsJons($"{BasePath}/update-cart", model);
        if(response.IsSuccessStatusCode)
            return await response.ReadContentAs<CartViewModel>(); 
        else 
            throw new Exception("Something went wrong when calling API");
    }
    public async Task<bool> RemoveFromCart(long id, string token)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token); 
        var response = await _client.DeleteAsync($"{BasePath}/remove-cart/{id}"); 
        if(response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>(); 
        else 
            throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> ApplyCoupon(CartViewModel model, string token)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token); 
        
        var response = await _client.PostAsJons($"{BasePath}/apply-coupon", model); 
        if(response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>(); 
        else
            throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> RemoveCoupon(string userId, string token)
    {
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token); 

        var response = await _client.DeleteAsync($"{BasePath}/remove-coupon/{userId}"); 
        if(response.IsSuccessStatusCode)
            return await response.ReadContentAs<bool>(); 
        else 
            throw new Exception("Something went wrong when calling API");
    }

    public Task<CartViewModel> Checkout(string token, CartHeaderViewModel cartHeaderVM)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ClearCart(string userId, string token)
    {
        throw new NotImplementedException();
    }

}
