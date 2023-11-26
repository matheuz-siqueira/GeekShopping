using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Model.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<EmailLog> Emails { get; set; }
}
