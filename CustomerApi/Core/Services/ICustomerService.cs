using CustomerApi.Core.Models;
using SharedModels;
using SharedModels.Order.Dtos;

namespace CustomerApi.Core.Services;

public interface ICustomerService : IService<Customer>
{
    bool SufficientCredit(int customerId);
    void NotifyCustomer(int customerId, OrderDto order, OrderRejectReason? reason);
    public IEnumerable<OrderDto> GetCustomerOrders(int customerId);
    bool OutstandingBills(int customerId);
}