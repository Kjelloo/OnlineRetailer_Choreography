using CustomerApi.Core.Models;
using CustomerApi.Core.Services;
using CustomerApi.Domain.Helpers;
using SharedModels;
using SharedModels.Customer;

namespace CustomerApi.Domain.Services;

public class CustomerConverter : IConverter<Customer, CustomerDto>
{
    public Customer Convert(CustomerDto model)
    {
        return new Customer
        {
            Id = model.Id,
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            BillingAddress = model.BillingAddress,
            ShippingAddress = model.ShippingAddress,
            CreditStanding = model.CreditStanding
        };
    }

    public CustomerDto Convert(Customer model)
    {
        return new CustomerDto
        {
            Id = model.Id,
            Name = model.Name,
            Email = model.Email,
            Phone = model.Phone,
            BillingAddress = model.BillingAddress,
            ShippingAddress = model.ShippingAddress,
            CreditStanding = model.CreditStanding
        };
    }
}