﻿using EasyNetQ;
using OrderApi.Core.Converters;
using OrderApi.Core.Models;
using SharedModels;
using SharedModels.Customer.Messages;
using SharedModels.Helpers;
using SharedModels.Order.Dtos;
using SharedModels.Order.Messages;

namespace OrderApi.Infrastructure.Messages;

public class MessagePublisher : IMessagePublisher, IDisposable
{
    private readonly IBus _bus;
    private readonly IOrderLineConverter _orderConverter;
    private readonly IConverter<OrderStatus, OrderStatusDto> _orderStatusConverter;

    public MessagePublisher(IOrderLineConverter orderConverter, IConverter<OrderStatus, OrderStatusDto> orderStatusConverter)
    {
        _bus = RabbitHutch.CreateBus(MessageConnectionHelper.ConnectionString);
        _orderConverter = orderConverter;
        _orderStatusConverter = orderStatusConverter;
    }

    public void Dispose()
    {
        _bus.Dispose();
    }

    public void PublishOrderCreatedMessage(int customerId, int orderId, IList<OrderLine> orderLines)
    {
        var message = new OrderCreatedMessage
        {
            CustomerId = customerId,
            OrderId = orderId,
            OrderLines = _orderConverter.Convert(orderLines)
        };
        
        Console.WriteLine("Publishing message: " + message);
        
        _bus.PubSub.Publish(message);
    }

    public void PublishOrderStatusChangedMessage(int customerId, OrderDto order, OrderStatus orderStatus, string topic)
    {
        var message = new OrderStatusChangedMessage
        {
            Order = order,
            CustomerId = customerId,
            OrderStatus = _orderStatusConverter.Convert(orderStatus)
        };
        
        Console.WriteLine($"Publishing message: {message} to topic: " + topic);
        
        _bus.PubSub.Publish(message, x => x.WithTopic(topic));
    }

    public void PublishCustomerCreditStandingChangedMessage(int customerId, int newCredit)
    {
        var message = new CustomerCreditStandingChangedMessage
        {
            CustomerId = customerId,
            Credit = newCredit
        };
        
        Console.WriteLine("Publishing message: " + message);
        
        _bus.PubSub.Publish(message);
    }
}