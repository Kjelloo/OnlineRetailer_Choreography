﻿using EasyNetQ;
using ProductApi.Core.Services;
using SharedModels.Order.Messages;

namespace ProductApi.Infrastructure.Messages;

public class MessageListener
{
    private readonly string _connectionString;
    private readonly IServiceProvider _provider;
    private IBus _bus;

    // The service provider is passed as a parameter, because the class needs
    // access to the product repository. With the service provider, we can create
    // a service scope that can provide an instance of the product repository.
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
            _bus.PubSub.Subscribe<OrderCreatedMessage>("productApiHkCreated",
                HandleOrderCreated);

            // Block the thread so that it will not exit and stop subscribing.
            lock (this)
            {
                Monitor.Wait(this);
            }
        }
    }

    private void HandleOrderCreated(OrderCreatedMessage message)
    {
        // A service scope is created to get an instance of the product repository.
        // When the service scope is disposed, the product repository instance will
        // also be disposed.
        using var scope = _provider.CreateScope();
        
        Console.WriteLine($"ProductApi received message: {message.OrderId}");

        var services = scope.ServiceProvider;
        var productService = services.GetService<IProductService>();

        // Check if order is valid
        var orderValidation = productService.IsOrderValid(message);

        var orderAccepted = orderValidation.Keys.First();
        var orderRejected = orderValidation.Values.First();

        if (orderAccepted)
        {
            // Reserve items and publish an OrderAcceptedMessage
            productService.ReserveProduct(message.OrderLines);

            var replyMessage = new OrderAcceptedMessage
            {
                OrderId = message.OrderId
            };
            
            Console.WriteLine($"ProductApi sending accepted message: {replyMessage.OrderId}");

            _bus.PubSub.Publish(replyMessage);
        }
        else
        {
            // Publish an OrderRejectedMessage
            var replyMessage = new OrderRejectedMessage
            {
                OrderId = message.OrderId,
                Reason = orderRejected
            };
            
            Console.WriteLine($"ProductApi sending rejected message: {replyMessage.OrderId} +  {replyMessage.Reason}");

            _bus.PubSub.Publish(replyMessage);
        }
    }
}