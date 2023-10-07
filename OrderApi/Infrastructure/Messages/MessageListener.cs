using EasyNetQ;
using OrderApi.Core.Models;
using OrderApi.Core.Services;
using SharedModels;
using SharedModels.Customer.Messages;
using SharedModels.Order.Dtos;
using SharedModels.Order.Messages;

namespace OrderApi.Infrastructure.Messages;

public class MessageListener
{
    private readonly string _connectionString;
    private readonly IServiceProvider _provider;
    private IBus _bus;
    private IConverter<Order, OrderDto> _orderConverter;

    // The service provider is passed as a parameter, because the class needs
    // access to the product repository. With the service provider, we can create
    // a service scope that can provide an instance of the order repository.
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
            _bus.PubSub.Subscribe<OrderAcceptedMessage>("orderApiHkAccepted",
                HandleOrderAccepted);

            _bus.PubSub.Subscribe<OrderRejectedMessage>("orderApiHkRejected",
                HandleOrderRejected);

            // Block the thread so that it will not exit and stop subscribing.
            lock (this)
            {
                Monitor.Wait(this);
            }
        }
    }

    private void HandleOrderAccepted(OrderAcceptedMessage message)
    {
        using var scope = _provider.CreateScope();
        var services = scope.ServiceProvider;
        _orderConverter = services.GetService<IConverter<Order, OrderDto>>();

        var orderRepos = services.GetService<IOrderService>();

        // Mark order as completed
        var order = orderRepos.Get(message.OrderId);
        order.Status = OrderStatus.WaitingToBeShipped;
        orderRepos.Edit(order);

        var customerOrderAcceptedMessage = new CustomerOrderUpdatedMessage
        {
            Order = _orderConverter.Convert(order),
            CustomerId = order.CustomerId
        };

        // Send accept message to customer service
        _bus.PubSub.Publish(customerOrderAcceptedMessage);
    }

    private void HandleOrderRejected(OrderRejectedMessage message)
    {
        using var scope = _provider.CreateScope();

        var services = scope.ServiceProvider;
        var orderRepos = services.GetService<IOrderService>();

        var order = orderRepos.Get(message.OrderId);

        var customerOrderRejectedMessage = new CustomerOrderRejectedMessage
        {
            Order = _orderConverter.Convert(order),
            CustomerId = order.CustomerId,
            OrderRejectReason = message.Reason
        };

        // Send reject message to customer service
        _bus.PubSub.Publish(customerOrderRejectedMessage);

        // Delete tentative order.
        orderRepos.Remove(order);
    }
}