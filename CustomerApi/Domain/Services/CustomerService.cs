using CustomerApi.Core.Models;
using CustomerApi.Core.Services;
using SharedModels;

namespace CustomerApi.Domain.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _repository;

    public CustomerService(IRepository<Customer> repository)
    {
        _repository = repository;
    }

    public Customer Add(Customer entity)
    {
        if (entity.Email is "" or null || entity.Name is "" or null || entity.Phone is 0 
            || entity.BillingAddress is "" or null || entity.ShippingAddress is "" or null 
            || entity.CreditStanding is 0)
        {
            throw new ArgumentException("Customer is missing required fields");
        }
        
        return _repository.Add(entity);
    }

    public IEnumerable<Customer> GetAll()
    {
        return _repository.GetAll();
    }

    public Customer Get(int id)
    {
        var customer = _repository.Get(id);

        if (customer is null)
        {
            throw new Exception("Customer does not exist");
        }
        
        return _repository.Get(id);
    }

    public Customer Edit(Customer entity)
    {
        return _repository.Edit(entity);
    }
    
    public Customer Remove(Customer entity)
    {
        return _repository.Remove(entity);
    }
    
    public bool SufficientCredit(int id)
    {
        return Get(id).CreditStanding > 670;
    }
}