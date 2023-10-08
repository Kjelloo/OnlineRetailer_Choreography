using CustomerApi.Core.Services;
using EasyNetQ;
using SharedModels.Customer.Messages;
using SharedModels.Order.Messages;

namespace CustomerApi.Infrastructure.Messages;

public class MessageListener
{
    private readonly string _connectionString;
    private readonly IServiceProvider _provider;
    private IBus _bus;

    public MessageListener(IServiceProvider provider, string connectionString)
    {
        _provider = provider;
        _connectionString = connectionString;
    }

    public void Start()
    {
        // Wait for RabbitMQ to start
        Thread.Sleep(10000);
        using (_bus = RabbitHutch.CreateBus(_connectionString))
        {
            // Handle rejected orders
            _bus.PubSub.Subscribe<CustomerOrderRejectedMessage>("customerApiOrderRejected",
                HandleCustomerOrderRejected);
            
            _bus.PubSub.Subscribe<OrderStatusChangedMessage>("customerApiOrderShipped", 
                HandleCustomerOrderAccepted, x => x.WithTopic("accepted"));

            // Handle shipped orders
            _bus.PubSub.Subscribe<OrderStatusChangedMessage>("customerApiOrderShipped", 
                HandleCustomerOrderShipped, x => x.WithTopic("shipped"));
            
            // Handle cancelled orders
            _bus.PubSub.Subscribe<OrderStatusChangedMessage>("customerApiOrderCancelled", 
                HandleCustomerOrderCancelled, x => x.WithTopic("cancelled"));
            
            // Handle credit standing changed
            _bus.PubSub.Subscribe<CustomerCreditStandingChangedMessage>("customerApiCreditStandingChanged",
                HandleCustomerCreditStandingChanged);

            // Block the thread so that it will not exit and stop subscribing.
            lock (this)
            {
                Monitor.Wait(this);
            }
        }
    }

    private void HandleCustomerOrderAccepted(OrderStatusChangedMessage message)
    {
        Console.WriteLine("Order accepted message received: " + message.Order.Id);
        
        using var scope = _provider.CreateScope();

        var service = scope.ServiceProvider;
        var customerService = service.GetService<ICustomerService>();

        if (customerService != null)
            customerService.NotifyCustomer(message.CustomerId, message.Order, null);
        else
            throw new ArgumentException("Could not notify customer about order being accepted");
    }

    private void HandleCustomerCreditStandingChanged(CustomerCreditStandingChangedMessage message)
    {
        Console.WriteLine("Credit standing changed message received: " + message.CustomerId + " Credit: " + message.Credit);
        
        using var scope = _provider.CreateScope();

        var service = scope.ServiceProvider;
        var customerService = service.GetService<ICustomerService>();

        if (customerService != null)
        {
            var customer = customerService.Get(message.CustomerId);
        
            // Add credit to customer
            customer.CreditStanding += message.Credit;
            customerService.Edit(customer);
        }
        else
        {
            throw new ArgumentException("Couldn't update credit standing");
        }
    }

    private void HandleCustomerOrderCancelled(OrderStatusChangedMessage message)
    {
        Console.WriteLine("Order cancelled message received: " + message.Order.Id);
        
        using var scope = _provider.CreateScope();

        var service = scope.ServiceProvider;
        var customerService = service.GetService<ICustomerService>();

        if (customerService != null)
            customerService.NotifyCustomer(message.CustomerId, message.Order, null);
        else
            throw new ArgumentException("Could not notify customer about order being cancelled");
    }

    private void HandleCustomerOrderShipped(OrderStatusChangedMessage message)
    {
        Console.WriteLine("Order shipped message received: " + message.Order.Id);
        
        using var scope = _provider.CreateScope();

        var service = scope.ServiceProvider;
        var customerService = service.GetService<ICustomerService>();

        if (customerService != null)
            customerService.NotifyCustomer(message.CustomerId, message.Order, null);
        else
            throw new ArgumentException("Could not notify customer about order being shipped");
    }

    private void HandleCustomerOrderRejected(CustomerOrderRejectedMessage message)
    {
        Console.WriteLine("Order rejected message received: " + message.Order.Id);
        
        using var scope = _provider.CreateScope();

        var service = scope.ServiceProvider;
        var customerService = service.GetService<ICustomerService>();

        if (customerService != null)
            customerService.NotifyCustomer(message.CustomerId, message.Order, message.OrderRejectReason);
        else
            throw new ArgumentException("Could not notify customer about order being rejected");
    }
}