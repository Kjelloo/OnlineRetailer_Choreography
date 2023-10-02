using CustomerApi.Core.Models;
using CustomerApi.Core.Services;
using CustomerApi.Domain.Repositories;

namespace CustomerApi.Domain.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _repository;

    public CustomerService(IRepository<Customer> repository)
    {
        _repository = repository;
    }

    public Customer Add(Customer customer)
    {
        if (customer.Email is "" or null || customer.Name is "" or null || customer.Phone is 0 
            || customer.BillingAddress is "" or null || customer.ShippingAddress is "" or null 
            || customer.CreditStanding is 0)
        {
            throw new ArgumentException("Customer is missing required fields");
        }
        
        return _repository.Add(customer);
    }

    public IEnumerable<Customer> Get()
    {
        return _repository.GetAll();
    }
}