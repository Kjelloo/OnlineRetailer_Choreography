using CustomerApi.Core.Models;
using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace CustomerApi.Infrastructure.EfCore;

public class CustomerApiContext : DbContext
{
    public CustomerApiContext(DbContextOptions<CustomerApiContext> options) 
        : base(options) { }

    public DbSet<Customer> Customers { get; set; }
}