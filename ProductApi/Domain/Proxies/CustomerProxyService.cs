using Dapr.Client;
using ProductApi.Core.Proxies;
using SharedModels.Customer;

namespace ProductApi.Domain.Proxies;

public class CustomerProxyService : ICustomerProxyService
{
    private readonly DaprClient _httpClient = new DaprClientBuilder().Build();
    
    public Task<CustomerDto> GetCustomer(int customerId)
    {
        try
        {
            var daprRequest = _httpClient.CreateInvokeMethodRequest(HttpMethod.Get, "customerapi", "Customers/" + customerId);
            var result = _httpClient.InvokeMethodAsync<CustomerDto>(daprRequest);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<bool> HasMinCredit(int customerId)
    {
        try
        {
            var daprRequest = _httpClient.CreateInvokeMethodRequest(HttpMethod.Get, "customerapi", "Customers/hasMinCredit/" + customerId);
            var result = _httpClient.InvokeMethodAsync<bool>(daprRequest);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<bool> HasOutstandingBills(int customerId)
    {
        try
        {
            var daprRequest = _httpClient.CreateInvokeMethodRequest(HttpMethod.Get, "customerapi", "Customers/bills/" + customerId);
            var result = _httpClient.InvokeMethodAsync<bool>(daprRequest);

            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}