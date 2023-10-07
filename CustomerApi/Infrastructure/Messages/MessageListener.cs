using EasyNetQ;

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
        using (bus = RabbitHutch.CreateBus(connectionString))
        {
            //bus.SendReceive.Receive("customer.requests", x => x
              //  .Add<CustomerExistsMessage>(HandleCustomerExistRequest));

            // Block the thread so that it will not exit and stop subscribing.
            lock (this)
            {
                Monitor.Wait(this);
            }
        }

    }
    
}