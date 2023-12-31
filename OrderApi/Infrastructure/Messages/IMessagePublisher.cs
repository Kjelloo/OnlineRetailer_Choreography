using OrderApi.Core.Models;
using SharedModels.Order.Dtos;
using SharedModels.Product;

namespace OrderApi.Infrastructure.Messages;

public interface IMessagePublisher
{
    void PublishOrderCreatedMessage(int customerId, int orderId,
        IList<OrderLine> orderLines);

    void PublishOrderStatusChangedMessage(int customerId, OrderDto order, OrderStatus orderStatus, string topic);
    void PublishCustomerCreditStandingChangedMessage(int customerId, int creditAdded);
    void PublishUpdateProductItemsMessage(OrderDto orderDto);
}