using GeekShopping.CartApi.Data.ValueObjects;

namespace GeekShopping.CartApi.Repository.Contracts;

public interface ICartRepository
{
    Task<CartVO> GetCartByUserId(string userId);
    Task<CartVO> SaveOrUpdateCart(CartVO cart); 
    Task<bool> RemoveFromCart(long cartDetailsId);
    Task<bool> ApplyCoupon(string userId, string couponCode); 
    Task<bool> RemoveCoupon(string userId); 
    Task<bool> ClearCart(string userId);   
}
