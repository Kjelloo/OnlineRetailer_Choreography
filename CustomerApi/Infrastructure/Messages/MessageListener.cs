using EasyNetQ;
using SharedModels.Customer;

namespace CustomerApi.Infrastructure.Messages;

public class MessageListener
{
    IServiceProvider provider;
    string connectionString;
    IBus bus;
    
    public MessageListener(IServiceProvider provider, string connectionString)
    {
        this.provider = provider;
        this.connectionString = connectionString;
    }
    
    public void Start()
    {
        Thread.Sleep(5000);
        using (bus = RabbitHutch.CreateBus(connectionString))
        {
            bus.PubSub.Subscribe<CustomerOrderAcceptedMessage>("customerApiOrderAccepted", HandleCustomerOrderAccepted);
            bus.PubSub.Subscribe<CustomerOrderRejectedMessage>("customerApiOrderRejected", HandleCustomerOrderRejected);

            // Block the thread so that it will not exit and stop subscribing.
            lock (this)
            {
                Monitor.Wait(this);
            }
        }

    }

    private void HandleCustomerOrderRejected(CustomerOrderRejectedMessage message)
    {
        throw new NotImplementedException();
    }

    private void HandleCustomerOrderAccepted(CustomerOrderAcceptedMessage message)
    {
        throw new NotImplementedException();
    }
}