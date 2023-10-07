using SharedModels;
using SharedModels.Order;

namespace OrderApi.Core.Services;

public interface IOrderService : IService<Order>
{
    IEnumerable<Order> FindByCustomer(int customerId);
    Order Cancel();
    Order Ship();
    Order Pay();
}