using CustomerApi.Core.Models;
using CustomerApi.Core.Services;
using CustomerApi.Domain.Repositories;
using SharedModels;

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

    public Customer Find(int id)
    {
        var customer = _repository.Get(id);

        if (customer is null)
        {
            throw new Exception("Customer does not exist");
        }
        
        return _repository.Get(id);
    }

    public bool SufficientCredit(int id)
    {
        return _repository.Get(id).CreditStanding > 670;
    }

    public IEnumerable<Customer> FindAll()
    {
        return _repository.GetAll();
    }
}