using Microsoft.EntityFrameworkCore;
using Shop.Core.Domain;

namespace Shop.Data
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }
        public DbSet<Spaceship> SpaceshipsKinder { get; set; }
        public DbSet<FileToApi> FileToApisKinder { get; set; }
        public DbSet<Kindergarden> Kindergardens { get; set; }
        public DbSet<FileToDatabase> FileToDataKinder { get; set; }
    }
}
