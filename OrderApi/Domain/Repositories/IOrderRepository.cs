using OrderApi.Core.Models;
using SharedModels;

namespace OrderApi.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        IEnumerable<Order> GetByCustomer(int customerId);
    }
}
