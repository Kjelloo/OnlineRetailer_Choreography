using OrderApi.Core.Models;
using SharedModels;

namespace OrderApi.Core.Services;

public interface IOrderService : IService<Order>
{
    IEnumerable<Order> GetByCustomer(int customerId);
    Order AcceptOrder(int id);
    Order Cancel(int id);
    Order Ship(int id);
    Order Pay(int id);
}