using CustomerApi.Core.Models;
using SharedModels;
using SharedModels.Order;
using SharedModels.Order.Models;

namespace CustomerApi.Core.Services;

public interface ICustomerService : IService<Customer>
{
    bool SufficientCredit(int id);
    void NotifyCustomer(int customerId, Order order, OrderRejectReason? reason);
    void SendEmail(string email, string subject, string body);
}