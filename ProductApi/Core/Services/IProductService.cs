using ProductApi.Core.Models;
using SharedModels.Order;

namespace ProductApi.Core.Services;

public interface IProductService
{
    Product Create(Product product);
    Product Find(int id);
    IEnumerable<Product> FindAll();
    Dictionary<bool, OrderRejectReason> IsOrderValid(OrderCreatedMessage orderCreatedMessage);
    void ReserveProduct(IEnumerable<OrderLine> orderLines);
}