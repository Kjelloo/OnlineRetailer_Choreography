using OrderApi.Core.Models;
using OrderApi.Core.Services;
using OrderApi.Domain.Repositories;
using OrderApi.Infrastructure.Messages;

namespace OrderApi.Domain.Services;

public class OrderService : IOrderService
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository, IMessagePublisher messagePublisher)
    {
        _messagePublisher = messagePublisher;
        _repository = repository;
    }

    public Order Add(Order order)
    {
        // Create a tentative order.
        order.Status = OrderStatus.Tentative;
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

    public IEnumerable<Order> GetByCustomer(int customerId)
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