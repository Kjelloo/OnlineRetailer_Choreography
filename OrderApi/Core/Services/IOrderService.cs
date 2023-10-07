using SharedModels;

namespace OrderApi.Core.Services;

public interface IOrderService
{
    Order Create(Order order);
    Order Find(int id);
    IEnumerable<Order> FindAll();
    IEnumerable<Order> FindByCustomer(int customerId);
    Order Cancel();
    Order Ship();
    Order Pay();
}