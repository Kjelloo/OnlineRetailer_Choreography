using OrderApi.Core.Models;
using SharedModels;
using SharedModels.Order.Dtos;

namespace OrderApi.Domain.Converters;

public class OrderStatusConverter : IConverter<OrderStatus, OrderStatusDto>
{
    public OrderStatus Convert(OrderStatusDto model)
    {
        switch (model)
        {
            case OrderStatusDto.Tentative:
                return OrderStatus.Tentative;
            case OrderStatusDto.WaitingToBePaid:
                return OrderStatus.WaitingToBePaid;
            case OrderStatusDto.WaitingToBeShipped:
                return OrderStatus.WaitingToBeShipped;
            case OrderStatusDto.Shipped:
                return OrderStatus.Shipped;
            case OrderStatusDto.Cancelled:
                return OrderStatus.Cancelled;
            case OrderStatusDto.Completed:
                return OrderStatus.Completed;
        }

        return default;
    }

    public OrderStatusDto Convert(OrderStatus model)
    {
        switch (model)
        {
            case OrderStatus.Tentative:
                return OrderStatusDto.Tentative;
            case OrderStatus.WaitingToBePaid:
                return OrderStatusDto.WaitingToBePaid;
            case OrderStatus.WaitingToBeShipped:
                return OrderStatusDto.WaitingToBeShipped;
            case OrderStatus.Shipped:
                return OrderStatusDto.Shipped;
            case OrderStatus.Cancelled:
                return OrderStatusDto.Cancelled;
            case OrderStatus.Completed:
                return OrderStatusDto.Completed;
        }

        return default;
    }
}