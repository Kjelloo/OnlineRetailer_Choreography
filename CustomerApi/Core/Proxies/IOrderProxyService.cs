using SharedModels.Order.Dtos;

namespace CustomerApi.Core.Proxies;

public interface IOrderProxyService
{
    Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(int customerId);
}