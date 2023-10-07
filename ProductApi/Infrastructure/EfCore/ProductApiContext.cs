using Microsoft.EntityFrameworkCore;
using ProductApi.Core.Models;

namespace ProductApi.Infrastructure.EfCore
{
    public class ProductApiContext : DbContext
    {
        public ProductApiContext(DbContextOptions<ProductApiContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
