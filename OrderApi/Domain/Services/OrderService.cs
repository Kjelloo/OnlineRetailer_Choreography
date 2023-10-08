using OrderApi.Core.Models;
using OrderApi.Core.Services;
using OrderApi.Domain.Repositories;
using OrderApi.Infrastructure.Messages;
using SharedModels;
using SharedModels.Order.Dtos;

namespace OrderApi.Domain.Services;

public class OrderService : IOrderService
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly IOrderRepository _repository;
    private readonly IConverter<Order, OrderDto> _orderConverter;

    public OrderService(IOrderRepository repository, IMessagePublisher messagePublisher, IConverter<Order, OrderDto> orderConverter)
    {
        _messagePublisher = messagePublisher;
        _orderConverter = orderConverter;
        _repository = repository;
    }

    public Order Add(Order order)
    {
        // Create a tentative order.
        order.Status = OrderStatus.Tentative;
        var newOrder = _repository.Add(order);

        // Update order lines with the new order id.
        foreach (var orderLine in newOrder.OrderLines)
        {
            orderLine.OrderId = newOrder.Id;
        }

        Edit(newOrder);
        
        // Publish OrderStatusChangedMessage. 
        _messagePublisher.PublishOrderCreatedMessage(
            newOrder.CustomerId, newOrder.Id, newOrder.OrderLines.ToList());

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

    public Order Cancel(int id)
    {
        var order = Get(id);
        order.Status = OrderStatus.Cancelled;
        var updatedOrder = Edit(order);

        var orderDto = _orderConverter.Convert(updatedOrder);
        
        _messagePublisher.PublishOrderStatusChangedMessage(
            updatedOrder.CustomerId, orderDto, updatedOrder.Status, "cancelled");

        return updatedOrder;
    }

    public Order Ship(int id)
    {
        var order = Get(id);
        order.Status = OrderStatus.Shipped;
        var updatedOrder = Edit(order);
        
        var orderDto = _orderConverter.Convert(updatedOrder);
        
        _messagePublisher.PublishOrderStatusChangedMessage(
            updatedOrder.CustomerId, orderDto, updatedOrder.Status, "shipped");

        return updatedOrder;
    }

    public Order Pay(int id)
    {
        var order = Get(id);
        order.Status = OrderStatus.Completed;
        var updatedOrder = Edit(order);
        
        _messagePublisher.PublishCustomerCreditStandingChangedMessage(
            updatedOrder.CustomerId, 20);

        return updatedOrder;
    }
}