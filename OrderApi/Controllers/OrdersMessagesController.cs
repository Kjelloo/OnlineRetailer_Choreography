using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Core.Models;
using OrderApi.Core.Services;
using SharedModels;
using SharedModels.Customer.Messages;
using SharedModels.Order.Dtos;
using SharedModels.Order.Messages;

namespace OrderApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersMessagesController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IConverter<Order, OrderDto> _converter;
    private readonly DaprClient _daprClient = new DaprClientBuilder().Build();


    public OrdersMessagesController(IOrderService orderService, IConverter<Order, OrderDto> converter)
    {
        _orderService = orderService;
        _converter = converter;
    }

    [Topic("pubsub", "orderApiOrderAccepted")]
    [HttpPost("orderAccepted")]
    public async void OrderAccepted(OrderAcceptedMessage message)
    {
        Console.WriteLine("Order accepted message received: " + message.OrderId);
        
        var order = _orderService.AcceptOrder(message.OrderId);
        
        var customerOrderAcceptedMessage = new OrderStatusChangedMessage
        {
            Order = _converter.Convert(order),
            CustomerId = order.CustomerId,
            OrderStatus = OrderStatusDto.WaitingToBeShipped
        };
        
        await _daprClient.PublishEventAsync("pubsub", "customerApiOrderAccepted", customerOrderAcceptedMessage);
        
        Console.WriteLine("Order accepted message sent: " + message.OrderId);
    }
    
    [Topic("pubsub", "orderApiOrderRejected")]
    [HttpPost("orderRejected")]
    public async void OrderRejected(OrderRejectedMessage message)
    {
        Console.WriteLine("Order rejected message received: " + message.OrderId);
        
        var order = _orderService.Get(message.OrderId);
        
        var customerOrderRejectedMessage = new CustomerOrderRejectedMessage
        {
            Order = _converter.Convert(order),
            CustomerId = order.CustomerId,
            OrderRejectReason = message.Reason
        };
        
        await _daprClient.PublishEventAsync("pubsub", "customerApiOrderRejected", customerOrderRejectedMessage);
        
        Console.WriteLine("Order rejected message sent for order: " + message.OrderId);
    }
}