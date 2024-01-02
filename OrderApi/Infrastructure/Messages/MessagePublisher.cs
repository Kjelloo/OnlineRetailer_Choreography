using Dapr.Client;
using OrderApi.Core.Converters;
using OrderApi.Core.Models;
using SharedModels;
using SharedModels.Customer.Messages;
using SharedModels.Order.Dtos;
using SharedModels.Order.Messages;
using SharedModels.Product;

namespace OrderApi.Infrastructure.Messages;

public class MessagePublisher : IMessagePublisher
{
    private readonly IOrderLineConverter _orderConverter;
    private readonly IConverter<OrderStatus, OrderStatusDto> _orderStatusConverter;
    private readonly DaprClient _daprClient = new DaprClientBuilder().Build();

    public MessagePublisher(IOrderLineConverter orderConverter, IConverter<OrderStatus, OrderStatusDto> orderStatusConverter)
    {
        _orderConverter = orderConverter;
        _orderStatusConverter = orderStatusConverter;
    }

    public async void PublishOrderCreatedMessage(int customerId, int orderId, IList<OrderLine> orderLines)
    {
        var message = new OrderCreatedMessage
        {
            CustomerId = customerId,
            OrderId = orderId,
            OrderLines = _orderConverter.Convert(orderLines)
        };

        await _daprClient.PublishEventAsync("pubsub", "productApiOrderCreated", message);
        Console.WriteLine("Publishing message for order: " + message.OrderId);
        
        // _bus.PubSub.Publish(message);
    }

    public async void PublishOrderStatusChangedMessage(int customerId, OrderDto order, OrderStatus orderStatus, string topic)
    {
        var message = new OrderStatusChangedMessage
        {
            Order = order,
            CustomerId = customerId,
            OrderStatus = _orderStatusConverter.Convert(orderStatus)
        };
        
        await _daprClient.PublishEventAsync("pubsub", topic, message);
        
        Console.WriteLine($"Publishing message with order: {message.Order.Id} to topic: " + topic);
        // _bus.PubSub.Publish(message, x => x.WithTopic(topic));
    }

    public async void PublishCustomerCreditStandingChangedMessage(int customerId, int newCredit)
    {
        var message = new CustomerCreditStandingChangedMessage
        {
            CustomerId = customerId,
            Credit = newCredit
        };
        
        // _bus.PubSub.Publish(message);
        await _daprClient.PublishEventAsync("pubsub", "customerApiCreditStandingChanged", message);
        
        Console.WriteLine("Publishing message for customer credit changed: " + message.CustomerId);
    }
    
    public async void PublishUpdateProductItemsMessage(OrderDto orderDto)
    {
        foreach (var order in orderDto.OrderLines)
        {
            var message = new ProductUpdateItemStockMessage
            {
                ProductId = order.ProductId,
                Quantity = order.Quantity
            };
            
            // _bus.PubSub.Publish(message);
            await _daprClient.PublishEventAsync("pubsub", "productApiUpdateItemStock", message);
            
            Console.WriteLine("Publishing update item quantity message for product: " + message.ProductId);
        }
    }
}