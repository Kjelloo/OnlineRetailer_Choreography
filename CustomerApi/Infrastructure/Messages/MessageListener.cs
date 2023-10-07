using CustomerApi.Core.Services;
using EasyNetQ;
using SharedModels.Customer.Messages;

namespace CustomerApi.Infrastructure.Messages;

public class MessageListener
{
    private readonly IServiceProvider _provider;
    private readonly string _connectionString;
    private IBus _bus;
    
    public MessageListener(IServiceProvider provider, string connectionString)
    {
        _provider = provider;
        _connectionString = connectionString;
    }
    
    public void Start()
    {
        // Wait for RabbitMQ to start
        Thread.Sleep(5000);
        using (_bus = RabbitHutch.CreateBus(_connectionString))
        {
            // Handle rejected orders
            _bus.PubSub.Subscribe<CustomerOrderRejectedMessage>("customerApiOrderRejected", HandleCustomerOrderRejected);
            
            // Handle updated orders
            _bus.PubSub.Subscribe<CustomerOrderUpdatedMessage>("customerApiOrderUpdated", HandleCustomerOrderUpdated);

            // Block the thread so that it will not exit and stop subscribing.
            lock (this)
            {
                Monitor.Wait(this);
            }
        }
    }

    private void HandleCustomerOrderUpdated(CustomerOrderUpdatedMessage message)
    {
        using var scope = _provider.CreateScope();
        
        var service = scope.ServiceProvider;
        var customerService = service.GetService<ICustomerService>();
        
        customerService.NotifyCustomer(message.CustomerId, message.Order, null);
    }

    private void HandleCustomerOrderRejected(CustomerOrderRejectedMessage message)
    {
        using var scope = _provider.CreateScope();
        
        var service = scope.ServiceProvider;
        var customerService = service.GetService<ICustomerService>();
        
        customerService.NotifyCustomer(message.CustomerId, message.Order, message.OrderRejectReason);
    }
}