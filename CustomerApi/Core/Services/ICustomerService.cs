using CustomerApi.Core.Models;
using SharedModels;

namespace CustomerApi.Core.Services;

public interface ICustomerService
{
    Customer Add(Customer customer);
    Customer Find(int id);
    bool SufficientCredit(int id);
    IEnumerable<Customer> FindAll();
}