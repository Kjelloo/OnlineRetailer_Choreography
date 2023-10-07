using Microsoft.EntityFrameworkCore;
using SharedModels;
using SharedModels.Order;

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
