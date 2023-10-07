using Microsoft.EntityFrameworkCore;
using OrderApi.Core.Models;

namespace OrderApi.Infrastructure.EfCore
{
    public class OrderApiContext : DbContext
    {
        public OrderApiContext(DbContextOptions<OrderApiContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
    }
}
