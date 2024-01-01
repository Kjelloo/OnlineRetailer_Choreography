using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApi.Core.Services;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Customer.Messages;
using SharedModels.Order.Messages;

namespace CustomerApi.Controllers;

[Route("[controller]")]
[ApiController]
public class CustomersMessagesController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersMessagesController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [Topic("pubsub", "customerApiOrderRejected")]
    [HttpPost("orderRejected")]
    public IActionResult OrderRejected(CustomerOrderRejectedMessage message)
    {
        Console.WriteLine("Order rejected message received: " + message.Order.Id);
        
        _customerService.NotifyCustomer(message.CustomerId, message.Order, message.OrderRejectReason);
        
        return Ok();
    }
    
    [Topic("pubsub", "customerApiOrderAccepted")]
    [HttpPost("orderAccepted")]
    public IActionResult OrderAccepted(OrderStatusChangedMessage message)
    {
        Console.WriteLine("Order accepted message received: " + message.Order.Id);
        
        _customerService.NotifyCustomer(message.CustomerId, message.Order, null);
        
        return Ok();
    }
    
    [Topic("pubsub", "customerApiOrderShipped")]
    [HttpPost("orderShipped")]
    public IActionResult OrderShipped(OrderStatusChangedMessage message)
    {
        Console.WriteLine("Order shipped message received: " + message.Order.Id);
        
        _customerService.NotifyCustomer(message.CustomerId, message.Order, null);
        
        return Ok();
    }
    
    [Topic("pubsub", "customerApiOrderCancelled")]
    [HttpPost("orderCancelled")]
    public IActionResult OrderCancelled(OrderStatusChangedMessage message)
    {
        Console.WriteLine("Order cancelled message received: " + message.Order.Id);
        
        _customerService.NotifyCustomer(message.CustomerId, message.Order, null);
        
        return Ok();
    }
    
    [Topic("pubsub", "customerApiCreditStandingChanged")]
    [HttpPost("creditStandingChanged")]
    public IActionResult CreditStandingChanged(CustomerCreditStandingChangedMessage message)
    {
        Console.WriteLine("Credit standing changed message received: " + message.CustomerId);
        
        var customer = _customerService.Get(message.CustomerId);
        
        // Add credit to customer
        customer.CreditStanding += message.Credit;
        _customerService.Edit(customer);
        
        return Ok();
    }
}