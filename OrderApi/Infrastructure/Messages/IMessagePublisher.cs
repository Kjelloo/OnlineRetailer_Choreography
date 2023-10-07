using SharedModels;
using SharedModels.Order;
using SharedModels.Order.Models;

namespace OrderApi.Infrastructure.Messages
{
    public interface IMessagePublisher
    {
        void PublishOrderCreatedMessage(int? customerId, int orderId,
            IList<OrderLine> orderLines);
    }
}
