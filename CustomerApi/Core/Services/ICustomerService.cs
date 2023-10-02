using CustomerApi.Core.Models;

namespace CustomerApi.Core.Services;

public interface ICustomerService
{
    Customer Add(Customer customer);
    IEnumerable<Customer> Get();
}