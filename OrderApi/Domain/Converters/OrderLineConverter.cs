using OrderApi.Core.Converters;
using OrderApi.Core.Models;
using SharedModels.Order.Dtos;

namespace OrderApi.Domain.Converters;

public class OrderLineConverter : IOrderLineConverter
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

    public IList<OrderLine> Convert(IList<OrderLineDto> models)
    {
        if (models is null)
            return new List<OrderLine>();
        
        return models.Select(Convert).ToList();
    }

    public IList<OrderLineDto> Convert(IList<OrderLine> models)
    {
        if (models is null)
            return new List<OrderLineDto>();
        
        return models.Select(Convert).ToList();
    }
}