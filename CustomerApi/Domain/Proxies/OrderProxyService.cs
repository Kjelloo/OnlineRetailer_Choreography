using System.Threading.Channels;
using CustomerApi.Core.Proxies;
using Dapr.Client;
using Grpc.Core;
using Grpc.Net.Client;
using SharedModels.Order.Dtos;

namespace CustomerApi.Domain.Proxies;

public class OrderProxyService : IOrderProxyService
{
    private readonly DaprClient _httpClient = new DaprClientBuilder().Build();

    public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(int customerId)
    {
        try
        {
            var daprRequest = _httpClient.CreateInvokeMethodRequest(HttpMethod.Get, "orderapi", "Orders/customerOrders/" + customerId);
            var result = await _httpClient.InvokeMethodAsync<IEnumerable<OrderDto>>(daprRequest);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}