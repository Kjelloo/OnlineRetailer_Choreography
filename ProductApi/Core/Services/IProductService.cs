using ProductApi.Core.Models;
using SharedModels;
using SharedModels.Order.Dtos;
using SharedModels.Order.Messages;

namespace ProductApi.Core.Services;

public interface IProductService : IService<Product>
{
    Dictionary<bool, OrderRejectReason> IsOrderValid(OrderCreatedMessage orderCreatedMessage);
    void ReserveProduct(IEnumerable<OrderLineDto> orderLines);
}