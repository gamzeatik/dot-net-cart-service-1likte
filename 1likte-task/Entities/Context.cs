using Microsoft.EntityFrameworkCore;

namespace _1likte_task.Entities;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options):base(options) { }
    
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}