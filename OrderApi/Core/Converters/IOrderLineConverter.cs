using OrderApi.Core.Models;
using SharedModels;
using SharedModels.Order.Dtos;

namespace OrderApi.Core.Converters;

public interface IOrderLineConverter : IConverter<OrderLine, OrderLineDto>
{
    IList<OrderLine> Convert(IList<OrderLineDto>? models);
    IList<OrderLineDto> Convert(IList<OrderLine>? models);
}