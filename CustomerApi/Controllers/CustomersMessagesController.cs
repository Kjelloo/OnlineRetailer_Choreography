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
    public void OrderAccepted(OrderStatusChangedMessage message)
    {
        Console.WriteLine("Order accepted message received for order: " + message.Order.Id);
        
        _customerService.NotifyCustomer(message.CustomerId, message.Order, null);
    }
    
    [Topic("pubsub", "customerApiOrderShipped")]
    [HttpPost("orderShipped")]
    public void OrderShipped(OrderStatusChangedMessage message)
    {
        Console.WriteLine("Order shipped message received for order: " + message.Order.Id);
        
        _customerService.NotifyCustomer(message.CustomerId, message.Order, null);
    }
    
    [Topic("pubsub", "customerApiOrderCancelled")]
    [HttpPost("orderCancelled")]
    public void OrderCancelled(OrderStatusChangedMessage message)
    {
        Console.WriteLine("Order cancelled message received for order: " + message.Order.Id);
        
        _customerService.NotifyCustomer(message.CustomerId, message.Order, null);
    }
    
    [Topic("pubsub", "customerApiCreditStandingChanged")]
    [HttpPost("creditStandingChanged")]
    public void CreditStandingChanged(CustomerCreditStandingChangedMessage message)
    {
        Console.WriteLine("Credit standing changed message received for order: " + message.CustomerId);
        
        var customer = _customerService.Get(message.CustomerId);
        
        // Add credit to customer
        customer.CreditStanding += message.Credit;
        _customerService.Edit(customer);
    }
}