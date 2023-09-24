using Microsoft.EntityFrameworkCore;
using nike_shoes_shop_backend.Models;

namespace nike_shoes_shop_backend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Story> Story { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<Users> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>().HasNoKey();
        }
    }
}