using ProductApi.Core.Models;
using SharedModels;
using SharedModels.Order;
using SharedModels.Order.Messages;
using SharedModels.Order.Models;

namespace ProductApi.Core.Services;

public interface IProductService : IService<Product>
{
    Dictionary<bool, OrderRejectReason> IsOrderValid(OrderCreatedMessage orderCreatedMessage);
    void ReserveProduct(IEnumerable<OrderLine> orderLines);
}