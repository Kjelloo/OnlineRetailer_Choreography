using CustomerApi.Core.Models;
using CustomerApi.Domain.Repositories;
using EasyNetQ;
using SharedModels;

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
            bus.SendReceive.Receive("customer.requests", x => x
                .Add<CustomerExistsMessage>(HandleCustomerExistRequest));

            // Block the thread so that it will not exit and stop subscribing.
            lock (this)
            {
                Monitor.Wait(this);
            }
        }

    }

    private void HandleCustomerExistRequest(CustomerExistsMessage customerExistsMessage)
    {
        using (var scope = provider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var customerRepo = services.GetService<IRepository<Customer>>();
            
            var customer = customerRepo.Get(customerExistsMessage.CustomerId);
            
            if (customer is null)
            {
                bus.SendReceive.Send("customer.requests", customerExistsMessage); 
            }
            else
            {
                customerExistsMessage.Exists = true;
                bus.SendReceive.Send("customer.requests", customerExistsMessage);
            }
                
        }
    }
}