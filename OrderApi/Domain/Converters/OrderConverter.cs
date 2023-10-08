using OrderApi.Core.Converters;
using OrderApi.Core.Models;
using SharedModels;
using SharedModels.Order.Dtos;

namespace OrderApi.Domain.Converters;

public class OrderConverter : IConverter<Order, OrderDto>
{
    private readonly IConverter<OrderStatus, OrderStatusDto> _orderStatusConverter;
    private readonly IOrderLineConverter _orderLineConverter;

    public OrderConverter(IConverter<OrderStatus, OrderStatusDto> orderStatusConverter, IOrderLineConverter orderLineConverter)
    {
        _orderStatusConverter = orderStatusConverter;
        _orderLineConverter = orderLineConverter;
    }

    public Order Convert(OrderDto model)
    {
        return new Order
        {
            Id = model.Id,
            CustomerId = model.CustomerId,
            OrderLines = _orderLineConverter.Convert(model.OrderLines.ToList()),
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
            OrderLines = _orderLineConverter.Convert(model.OrderLines.ToList()),
            Status = _orderStatusConverter.Convert(model.Status),
            Date = model.Date
        };
    }
}