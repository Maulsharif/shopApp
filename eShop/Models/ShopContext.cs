using Microsoft.EntityFrameworkCore;

namespace eShop.Models
{
    public class ShopContext:DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        
        public ShopContext(DbContextOptions<ShopContext> options) : base(options){}
    }
}