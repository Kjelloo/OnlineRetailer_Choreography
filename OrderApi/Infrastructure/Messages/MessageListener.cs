using EasyNetQ;
using OrderApi.Domain.Repositories;
using SharedModels;
using SharedModels.Customer;
using SharedModels.Order;

namespace OrderApi.Infrastructure.Messages
{
    public class MessageListener
    {
        IServiceProvider provider;
        string connectionString;
        IBus bus;

        // The service provider is passed as a parameter, because the class needs
        // access to the product repository. With the service provider, we can create
        // a service scope that can provide an instance of the order repository.
        public MessageListener(IServiceProvider provider, string connectionString)
        {
            this.provider = provider;
            this.connectionString = connectionString;
        }

        public void Start()
        {
            using (bus = RabbitHutch.CreateBus(connectionString))
            {
                bus.PubSub.Subscribe<OrderAcceptedMessage>("orderApiHkAccepted",
                    HandleOrderAccepted);

                bus.PubSub.Subscribe<OrderRejectedMessage>("orderApiHkRejected",
                    HandleOrderRejected);

                // Block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }

        }

        private void HandleOrderAccepted(OrderAcceptedMessage message)
        {
            using var scope = provider.CreateScope();
            
            var services = scope.ServiceProvider;
            var orderRepos = services.GetService<IRepository<Order>>();

            // Mark order as completed
            var order = orderRepos.Get(message.OrderId);
            order.Status = Order.OrderStatus.WaitingToBeShipped;
            orderRepos.Edit(order);
            
            var customerOrderAcceptedMessage = new CustomerOrderAcceptedMessage
            {
                OrderId = message.OrderId,
                CustomerId = order.CustomerId
            };

            // Send accept message to customer service
            bus.PubSub.Publish(customerOrderAcceptedMessage);
        }

        private void HandleOrderRejected(OrderRejectedMessage message)
        {
            using var scope = provider.CreateScope();
            
            var services = scope.ServiceProvider;
            var orderRepos = services.GetService<IRepository<Order>>();

            var order = orderRepos.Get(message.OrderId);
                
            var customerOrderRejectedMessage = new CustomerOrderRejectedMessage
            {
                OrderId = message.OrderId,
                CustomerId = order.CustomerId,
                OrderRejectReason = message.Reason
            };

            // Send reject message to customer service
            bus.PubSub.Publish(customerOrderRejectedMessage);
                
            // Delete tentative order.
            orderRepos.Remove(message.OrderId);
        }
    }
}
