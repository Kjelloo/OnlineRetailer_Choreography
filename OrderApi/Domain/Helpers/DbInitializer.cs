using OrderApi.Infrastructure.EfCore;
using SharedModels.Order.Models;

namespace OrderApi.Domain.Helpers
{
    public class DbInitializer : IDbInitializer
    {
        // This method will create and seed the database.
        public void Initialize(OrderApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Orders
            if (context.Orders.Any())
            {
                return;   // DB has been seeded
            }

            List<Order> orders = new List<Order>
            {
                new Order {
                    Date = DateTime.Today,
                    CustomerId = 1,
                    Status = OrderStatus.Completed,
                    OrderLines = new List<OrderLine>{
                        new OrderLine { ProductId = 1, Quantity = 2 } }
                },
                new Order {
                    Date = DateTime.Today-TimeSpan.FromDays(1),
                    CustomerId = 1,
                    Status = OrderStatus.Completed,
                    OrderLines = new List<OrderLine>{
                        new OrderLine { ProductId = 2, Quantity = 2 } }
                }
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
    }
}
