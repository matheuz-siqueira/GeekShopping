using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.Contracts;

public interface ICartService
{
    Task<CartViewModel> GetCartByUserId(string userId, string token);
    Task<CartViewModel> AddItemToCart(CartViewModel cart, string token); 
    Task<CartViewModel> UpdateCart(CartViewModel cart, string token); 
    Task<bool> RemoveFromCart(long id, string token); 

    Task<bool> ApplyCoupon(CartViewModel cart, string couponCode, string token); 
    Task<bool> RemoveCoupon(string userId, string token); 
    Task<bool> ClearCart(string userId, string token); 
    Task<CartViewModel> Checkout(string token, CartHeaderViewModel cartHeaderVM); 
}
