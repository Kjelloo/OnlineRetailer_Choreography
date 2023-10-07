using OrderApi.Core.Models;
using SharedModels;

namespace OrderApi.Core.Services;

public interface IOrderService : IService<Order>
{
    IEnumerable<Order> GetByCustomer(int customerId);
    Order Cancel();
    Order Ship();
    Order Pay();
}