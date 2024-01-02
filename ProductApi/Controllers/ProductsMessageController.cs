using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Core.Services;
using SharedModels.Order.Messages;
using SharedModels.Product;

namespace ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsMessageController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly DaprClient _daprClient = new DaprClientBuilder().Build();

    public ProductsMessageController(IProductService productService)
    {
        _productService = productService;
    }
    
    [Topic("pubsub", "productApiUpdateItemStock")]
    [HttpPost("updateItemStock")]
    public void UpdateItemStock(ProductUpdateItemStockMessage message)
    {
        Console.WriteLine($"ProductApi item update received message for product: {message.ProductId}");

        var product = _productService.Get(message.ProductId);
        product.ItemsReserved -= message.Quantity;
        product.ItemsInStock -= message.Quantity;
        _productService.Edit(product);
    }
    
    [Topic("pubsub", "productApiOrderCreated")]
    [HttpPost("orderCreated")]
    public async void OrderCreated(OrderCreatedMessage message)
    {
        Console.WriteLine($"ProductApi received message: {message.OrderId}");
        
        var orderValidation = _productService.IsOrderValid(message);

        var orderAccepted = orderValidation.Keys.First();
        var orderRejected = orderValidation.Values.First();

        if (orderAccepted)
        {
            // Reserve items and publish an OrderAcceptedMessage
            _productService.ReserveProduct(message.OrderLines);

            var replyMessage = new OrderAcceptedMessage
            {
                OrderId = message.OrderId
            };
            
            Console.WriteLine($"ProductApi sending accepted message for order: {replyMessage.OrderId}");

            await _daprClient.PublishEventAsync("pubsub", "orderApiOrderAccepted", replyMessage);
        }
        else
        {
            // Publish an OrderRejectedMessage
            var replyMessage = new OrderRejectedMessage
            {
                OrderId = message.OrderId,
                Reason = orderRejected
            };
            
            Console.WriteLine($"ProductApi sending rejected message for order: {replyMessage.OrderId} +  {replyMessage.Reason}");

            await _daprClient.PublishEventAsync("pubsub", "orderApiOrderRejected", replyMessage);
        }
    }
}