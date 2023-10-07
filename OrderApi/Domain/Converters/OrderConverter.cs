using OrderApi.Core.Models;
using SharedModels;
using SharedModels.Order.Dtos;

namespace OrderApi.Domain.Converters;

public class OrderConverter : IConverter<Order, OrderDto>
{
    private readonly IConverter<OrderStatus, OrderStatusDto> _orderStatusConverter;

    public OrderConverter(IConverter<OrderStatus, OrderStatusDto> orderStatusConverter)
    {
        _orderStatusConverter = orderStatusConverter;
    }

    public Order Convert(OrderDto model)
    {
        return new Order
        {
            Id = model.Id,
            CustomerId = model.CustomerId,
            OrderLines = model.OrderLines.Select(ol => new OrderLine
            {
                ProductId = ol.ProductId,
                Quantity = ol.Quantity
            }).ToList(),
            Status = _orderStatusConverter.Convert(model.Status),
            Date = model.Date
        };
    }

    public OrderDto Convert(Order model)
    {
        return new OrderDto
        {
            CustomerId = model.CustomerId,
            Id = model.Id,
            OrderLines = model.OrderLines.Select(ol => new OrderLineDto
            {
                ProductId = ol.ProductId,
                Quantity = ol.Quantity
            }).ToList(),
            Status = _orderStatusConverter.Convert(model.Status),
            Date = model.Date
        };
    }
}