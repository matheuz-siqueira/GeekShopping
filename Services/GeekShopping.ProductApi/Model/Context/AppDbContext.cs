using Microsoft.EntityFrameworkCore;

namespace GeekShopping.ProductApi.Model.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(){}
    public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) {}

    public DbSet<Product> Products { get; set; }

}
