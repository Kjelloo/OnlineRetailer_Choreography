using EasyNetQ;
using SharedModels;
using SharedModels.Order;

namespace OrderApi.Infrastructure.Messages
{
    public class MessagePublisher : IMessagePublisher, IDisposable
    {
        IBus bus;

        public MessagePublisher(string connectionString)
        {
            // Wait for RabbitMQ to start
            Thread.Sleep(5000);
            bus = RabbitHutch.CreateBus(connectionString);
        }

        public void Dispose()
        {
            bus.Dispose();
        }

        public void PublishOrderCreatedMessage(int? customerId, int orderId, IList<OrderLine> orderLines)
        {
            var message = new OrderCreatedMessage
            { 
                CustomerId = customerId,
                OrderId = orderId,
                OrderLines = orderLines 
            };

            bus.PubSub.Publish(message);
        }
    }
}
