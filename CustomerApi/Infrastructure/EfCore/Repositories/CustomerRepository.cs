using CustomerApi.Core.Models;
using Microsoft.EntityFrameworkCore;
using SharedModels;

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

    public Customer Edit(Customer entity)
    {
        db.Entry(entity).State = EntityState.Modified;
        db.SaveChanges();
        return entity;
    }

    public Customer Remove(Customer customer)
    {
        var customerRemove = db.Customers.FirstOrDefault(customer);
        db.Customers.Remove(customerRemove);
        db.SaveChanges();
        return customerRemove;
    }
}