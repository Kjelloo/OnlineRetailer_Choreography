using SharedModels;

namespace OrderApi.Infrastructure.Messages
{
    public interface IMessagePublisher
    {
        void PublishOrderCreatedMessage(int? customerId, int orderId,
            IList<OrderLine> orderLines);
    }
}
