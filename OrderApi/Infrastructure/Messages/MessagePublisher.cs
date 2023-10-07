using EasyNetQ;
using OrderApi.Core.Models;
using SharedModels;
using SharedModels.Helpers;
using SharedModels.Order.Dtos;
using SharedModels.Order.Messages;

namespace OrderApi.Infrastructure.Messages;

public class MessagePublisher : IMessagePublisher, IDisposable
{
    private readonly IBus _bus;
    private readonly IConverter<OrderLine, OrderLineDto> _orderConverter;

    public MessagePublisher(IConverter<OrderLine, OrderLineDto> orderConverter)
    {
        // Wait for RabbitMQ to start
        Thread.Sleep(5000);
        _orderConverter = orderConverter;
        _bus = RabbitHutch.CreateBus(MessageConnectionHelper.ConnectionString);
    }

    public void Dispose()
    {
        _bus.Dispose();
    }

    public void PublishOrderCreatedMessage(int? customerId, int orderId, IList<OrderLine> orderLines)
    {
        var message = new OrderCreatedMessage
        {
            CustomerId = customerId,
            OrderId = orderId,
            OrderLines = orderLines.Select(ol => _orderConverter.Convert(ol)).ToList()
        };

        _bus.PubSub.Publish(message);
    }
}