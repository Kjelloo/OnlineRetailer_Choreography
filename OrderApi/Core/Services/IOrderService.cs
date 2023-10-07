using SharedModels;
using SharedModels.Order;
using SharedModels.Order.Models;

namespace OrderApi.Core.Services;

public interface IOrderService : IService<Order>
{
    IEnumerable<Order> FindByCustomer(int customerId);
    Order Cancel();
    Order Ship();
    Order Pay();
}