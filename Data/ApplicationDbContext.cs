using Microsoft.EntityFrameworkCore;
using ProductManagementSystem.Models;

namespace ProductManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }

}
