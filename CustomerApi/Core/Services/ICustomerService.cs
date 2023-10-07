using CustomerApi.Core.Models;
using SharedModels;
using SharedModels.Order;
using SharedModels.Order.Models;

namespace CustomerApi.Core.Services;

public interface ICustomerService : IService<Customer>
{
    bool SufficientCredit(int customerId);
    void NotifyCustomer(int customerId, Order order, OrderRejectReason? reason);
    bool OutstandingBills(int customerId);
}