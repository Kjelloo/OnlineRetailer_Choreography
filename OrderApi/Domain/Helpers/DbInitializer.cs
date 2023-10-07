using OrderApi.Core.Models;
using OrderApi.Infrastructure.EfCore;

namespace OrderApi.Domain.Helpers;

public class DbInitializer : IDbInitializer
{
    // This method will create and seed the database.
    public void Initialize(OrderApiContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Look for any Orders
        if (context.Orders.Any()) return; // DB has been seeded

        var orders = new List<Order>
        {
            new()
            {
                Date = DateTime.Today,
                CustomerId = 1,
                Status = OrderStatus.Completed,
                OrderLines = new List<OrderLine>
                {
                    new() { ProductId = 1, Quantity = 2 }
                }
            },
            new()
            {
                Date = DateTime.Today - TimeSpan.FromDays(1),
                CustomerId = 1,
                Status = OrderStatus.Completed,
                OrderLines = new List<OrderLine>
                {
                    new() { ProductId = 2, Quantity = 2 }
                }
            }
        };

        context.Orders.AddRange(orders);
        context.SaveChanges();
    }
}