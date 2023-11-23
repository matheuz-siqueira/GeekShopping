using AutoMapper;
using GeekShopping.CartApi.Data.ValueObjects;
using GeekShopping.CartApi.Model;
using GeekShopping.CartApi.Model.Context;
using GeekShopping.CartApi.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.CartApi.Repository;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    public CartRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public Task<bool> ApplyCoupon(string userId, string couponCode)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ClearCart(string userId)
    {
        var cartHeader = await _context.CartHeaders
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if(cartHeader is not null)
        {
            _context.CartDetails
                .RemoveRange(_context.CartDetails.Where(c => c.CartHeaderId == cartHeader.Id));
            
            _context.CartHeaders.Remove(cartHeader); 
            await _context.SaveChangesAsync();
            return true; 
        }

        return false;
    }

    public async Task<CartVO> GetCartByUserId(string userId)
    {
        Cart cart = new()
        {
            CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId),
        };
        cart.CartDetails = _context.CartDetails
        .Include(c => c.Product)
            .Where(c => c.CartHeaderId == cart.CartHeader.Id);
        return _mapper.Map<CartVO>(cart);
    }

    public Task<bool> RemoveCoupon(string userId)
    {
        return null;
    }

    public async Task<bool> RemoveFromCart(long cartDetailsId)
    {
        try 
        {
            CartDetail cartDetail = await _context.CartDetails
                .FirstOrDefaultAsync(c => c.Id == cartDetailsId);

            int total = _context.CartDetails
                .Where(c => c.CartHeaderId == cartDetail.CartHeaderId).Count(); 

            _context.CartDetails.Remove(cartDetail);
            if(total == 1)
            {
                var cartHeaderToRemove = await _context.CartHeaders
                    .FirstOrDefaultAsync(c => c.Id == cartDetail.CartHeaderId);

                _context.CartHeaders.Remove(cartHeaderToRemove);  
            }
            await _context.SaveChangesAsync();
            return true; 
        }
        catch(Exception)
        {
            return false;
        }
    }

    public async Task<CartVO> SaveOrUpdateCart(CartVO vo)
    {
        Cart cart = _mapper.Map<Cart>(vo);
        var product = await _context.Products.FirstOrDefaultAsync(
            p => p.Id == vo.CartDetails.FirstOrDefault().ProductId);

        if(product is null)
        {
            _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);
            await _context.SaveChangesAsync();
        }

        var cartHeader = await _context.CartHeaders.AsNoTracking().FirstOrDefaultAsync(
                c => c.UserId == cart.CartHeader.UserId);
        
        if(cartHeader is null)
        {
            _context.CartHeaders.Add(cart.CartHeader);
            await _context.SaveChangesAsync();
            cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
            cart.CartDetails.FirstOrDefault().Product = null; 
            _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
        else 
        {
            var cartDetail = await _context.CartDetails.AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProductId == vo.CartDetails.FirstOrDefault()
                    .ProductId && p.CartHeaderId == cartHeader.Id); 

            if(cartDetail is null)
            {
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
                cart.CartDetails.FirstOrDefault().Product = null; 
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());   
                await _context.SaveChangesAsync();
            }
            else
            {
                cart.CartDetails.FirstOrDefault().Product = null; 
                cart.CartDetails.FirstOrDefault().Count += cartDetail.Count; 
                cart.CartDetails.FirstOrDefault().Id = cartDetail.Id;
                cart.CartDetails.FirstOrDefault().CartHeaderId = cartDetail.CartHeaderId; 
                _context.CartDetails.Update(cart.CartDetails.FirstOrDefault()); 
            }
        }

        return _mapper.Map<CartVO>(cart); 
    }
}