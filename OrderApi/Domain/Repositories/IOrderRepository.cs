using SharedModels;
using SharedModels.Order;

namespace OrderApi.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetByCustomer(int customerId);
    }
}
