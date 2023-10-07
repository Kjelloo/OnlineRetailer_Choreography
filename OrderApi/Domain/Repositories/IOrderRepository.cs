using SharedModels;
using SharedModels.Order;
using SharedModels.Order.Models;

namespace OrderApi.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetByCustomer(int customerId);
    }
}
