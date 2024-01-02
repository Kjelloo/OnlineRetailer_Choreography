using CustomerApi.Core.Models;
using CustomerApi.Core.Proxies;
using CustomerApi.Core.Services;
using SharedModels;
using SharedModels.Order.Dtos;

namespace CustomerApi.Domain.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _repository;
    private readonly IOrderProxyService _proxyService;

    public CustomerService(IRepository<Customer> repository, IOrderProxyService proxyService)
    {
        _repository = repository;
        _proxyService = proxyService;
    }

    public Customer Add(Customer entity)
    {
        if (entity.Email is "" or null || entity.Name is "" or null || entity.Phone is 0
            || entity.BillingAddress is "" or null || entity.ShippingAddress is "" or null
            || entity.CreditStanding is 0)
            throw new ArgumentException("Customer is missing required fields");

        return _repository.Add(entity);
    }

    public IEnumerable<Customer> GetAll()
    {
        return _repository.GetAll();
    }

    public Customer Get(int id)
    {
        var customer = _repository.Get(id);

        if (customer is null) throw new Exception("Customer does not exist");

        return _repository.Get(id);
    }

    public Customer Edit(Customer entity)
    {
        return _repository.Edit(entity);
    }

    public Customer Remove(Customer entity)
    {
        return _repository.Remove(entity);
    }

    public bool SufficientCredit(int customerId)
    {
        return Get(customerId).CreditStanding > 670;
    }

    public void NotifyCustomer(int customerId, OrderDto order, OrderRejectReason? reason)
    {
        var customer = Get(customerId);

        // If the order was rejected, notify the customer
        if (reason is not null)
        {
            SendEmail(customer.Email, "Order rejected", $"Your order {order.Id} was rejected because {reason}");
        }
        
        switch (order.Status)
        {
            case OrderStatusDto.WaitingToBePaid:
                SendEmail(customer.Email, "Order accepted", $"Your order {order.Id} was accepted");
                break;
            case OrderStatusDto.WaitingToBeShipped:
                SendEmail(customer.Email, "Order received", $"Your order {order.Id} was received and is being processed");
                break;
            case OrderStatusDto.Shipped:
                SendEmail(customer.Email, "Order shipped", $"Your order {order.Id} was shipped");
                break;
            case OrderStatusDto.Completed:
                SendEmail(customer.Email, "Order completed", $"Your order {order.Id} was completed");
                break;
            case OrderStatusDto.Cancelled:
                SendEmail(customer.Email, "Order cancelled", $"Your order {order.Id} was cancelled");
                break;
            case OrderStatusDto.Tentative:
                break;
            default:
                throw new ArgumentException("Order status is not valid");
        }
    }

    public bool OutstandingBills(int customerId)
    {
        // get customer - if customer does not exist, throw exception
        Get(customerId);

        return GetCustomerOrders(customerId).Any(order => order.Status is not OrderStatusDto.Completed);
    }

    public IEnumerable<OrderDto> GetCustomerOrders(int customerId)
    {
        var orderResponse = _proxyService.GetOrdersByCustomerId(customerId).Result;

        return orderResponse;
    }

    private static void SendEmail(string email, string subject, string body)
    {
        // Implement email sending
        Console.WriteLine("Sending email to {0} \nSubject: {1} \nBody: {2}", email, subject, body);
    }
}