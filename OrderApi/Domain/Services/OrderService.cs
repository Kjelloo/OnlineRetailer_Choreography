using System.Net;
using OrderApi.Core.Services;
using OrderApi.Domain.Repositories;
using OrderApi.Infrastructure.Messages;
using RestSharp;
using SharedModels;
using SharedModels.Helpers;
using SharedModels.Order;

namespace OrderApi.Domain.Services;

public class OrderService : IOrderService
{
    private readonly MessageListener _messageListener;
    private readonly IMessagePublisher _messagePublisher;
    private readonly IOrderRepository _repository;
    private RestClient customerClient;
    private RestClient productClient;

    public OrderService(IOrderRepository repository, IMessagePublisher messagePublisher, MessageListener messageListener)
    {
        _messageListener = messageListener;
        _messagePublisher = messagePublisher;
        _repository = repository;
        customerClient = new RestClient(RestConnectionHelper.GetCustomerUrl());
        productClient = new RestClient(RestConnectionHelper.GetProductUrl());
    }

    public Order Create(Order order)
    {
        // Create a tentative order.
        order.Status = Order.OrderStatus.Tentative;
        var newOrder = _repository.Add(order);
        
        // Publish OrderStatusChangedMessage. 
        _messagePublisher.PublishOrderCreatedMessage(
            newOrder.CustomerId, newOrder.Id, newOrder.OrderLines);
        
        return newOrder;
    }

    public Order Find(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Order> FindAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Order> FindByCustomer(int customerId)
    {
        return _repository.GetByCustomer(customerId);
    }

    public Order Cancel()
    {
        throw new NotImplementedException();
    }

    public Order Ship()
    {
        throw new NotImplementedException();
    }

    public Order Pay()
    {
        throw new NotImplementedException();
    }
}