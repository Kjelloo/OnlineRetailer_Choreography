using SharedModels.Customer;

namespace ProductApi.Core.Proxies;

public interface ICustomerProxyService
{
    Task<CustomerDto> GetCustomer(int customerId);
    Task<bool> HasMinCredit(int customerId);
    Task<bool> HasOutstandingBills(int customerId);
}