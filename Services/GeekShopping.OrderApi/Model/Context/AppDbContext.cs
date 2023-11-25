using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderApi.Model.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<OrderDetail> Details { get; set; }
    public DbSet<OrderHeader> Headers{ get; set; }
}

