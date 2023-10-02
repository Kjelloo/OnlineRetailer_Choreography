using CustomerApi.Core.Models;
using CustomerApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Infrastructure.EfCore.Repositories;

public class CustomerRepository : IRepository<Customer>
{
    private readonly CustomerApiContext db;

    public CustomerRepository(CustomerApiContext context)
    {
        db = context;
    }

    public Customer Add(Customer entity)
    {
        var newCustomer = db.Customers.Add(entity).Entity;
        db.SaveChanges();
        return newCustomer;
    }
    
    public Customer Get(int customerId)
    {
        return db.Customers.FirstOrDefault(p => p.Id == customerId)!;
    }

    public IEnumerable<Customer> GetAll()
    {
        return db.Customers;
    }

    public void Update(Customer entity)
    {
        db.Entry(entity).State = EntityState.Modified;
        db.SaveChanges();
    }

    public void Delete(Customer entity)
    {
        var product = db.Customers.FirstOrDefault(entity);
        db.Customers.Remove(product);
        db.SaveChanges();
    }
}