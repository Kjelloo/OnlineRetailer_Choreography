using CustomerApi.Core.Models;
using SharedModels;

namespace CustomerApi.Core.Services;

public interface ICustomerService : IService<Customer>
{
    bool SufficientCredit(int id);
}