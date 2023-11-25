using GeekShopping.OrderApi.Model.Context;
using GeekShopping.OrderApi.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderApi.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly DbContextOptions<AppDbContext> _context; 

    public OrderRepository(DbContextOptions<AppDbContext> context)
    {
        _context = context;
    }

    public async Task<bool> AddOrder(OrderHeader orderHeader)
    {
        if(orderHeader is null)
            return false; 

        await using var _db = new AppDbContext(_context);
        _db.Headers.Add(orderHeader);
        await _db.SaveChangesAsync();
        return true; 
    }

    public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool status)
    {
        await using var _db = new AppDbContext(_context);
        var header = await _db.Headers.FirstOrDefaultAsync(o => o.Id == orderHeaderId);
        if(header is not null)
        {
            header.PaymentStatus = status;
            await _db.SaveChangesAsync();  
        }    
    }
}
