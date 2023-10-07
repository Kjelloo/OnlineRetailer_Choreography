using OrderApi.Core.Models;
using SharedModels;
using SharedModels.Order.Dtos;

namespace OrderApi.Domain.Converters;

public class OrderLineConverter : IConverter<OrderLine, OrderLineDto>
{
    public OrderLine Convert(OrderLineDto model)
    {
        return new OrderLine
        {
            Id = model.Id,
            ProductId = model.ProductId,
            Quantity = model.Quantity,
            OrderId = model.OrderId
        };
    }

    public OrderLineDto Convert(OrderLine model)
    {
        return new OrderLineDto
        {
            Id = model.Id,
            ProductId = model.ProductId,
            Quantity = model.Quantity,
            OrderId = model.OrderId
        };
    }
}