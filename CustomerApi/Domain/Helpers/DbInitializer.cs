using CustomerApi.Core.Models;
using CustomerApi.Infrastructure.EfCore;

namespace CustomerApi.Domain.Helpers;

public class DbInitializer : IDbInitializer
{
    public void Initialize(CustomerApiContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var customers = new List<Customer>
        {
            new()
            {
                Id = 1,
                Email = "customer@customer.com",
                Name = "John Smith",
                Phone = 12345678,
                BillingAddress = "Some Billing Address",
                ShippingAddress = "Some Shipping Address",
                CreditStanding = 700
            }
        };

        context.Customers.AddRange(customers);
        context.SaveChanges();
    }
}