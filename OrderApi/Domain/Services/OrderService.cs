using OrderApi.Core.Services;
using OrderApi.Domain.Repositories;
using OrderApi.Infrastructure.Messages;
using RestSharp;
using SharedModels.Helpers;
using SharedModels.Order;

namespace OrderApi.Domain.Services;

public class OrderService : IOrderService
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly IOrderRepository _repository;
    private RestClient customerClient;
    private RestClient productClient;

    public OrderService(IOrderRepository repository, IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
        _repository = repository;
        customerClient = new RestClient(RestConnectionHelper.GetCustomerUrl());
        productClient = new RestClient(RestConnectionHelper.GetProductUrl());
    }

    public Order Add(Order order)
    {
        // Create a tentative order.
        order.Status = Order.OrderStatus.Tentative;
        var newOrder = _repository.Add(order);
        
        // Publish OrderStatusChangedMessage. 
        _messagePublisher.PublishOrderCreatedMessage(
            newOrder.CustomerId, newOrder.Id, newOrder.OrderLines);
        
        return newOrder;
    }

    public Order Get(int id)
    {
        return _repository.Get(id);
    }

    public IEnumerable<Order> GetAll()
    {
        return _repository.GetAll();
    }

    public Order Edit(Order entity)
    {
        return _repository.Edit(entity);
    }

    public Order Remove(Order entity)
    {
        return _repository.Remove(entity);
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