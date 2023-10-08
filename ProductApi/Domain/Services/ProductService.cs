using ProductApi.Core.Models;
using ProductApi.Core.Services;
using RestSharp;
using SharedModels;
using SharedModels.Customer;
using SharedModels.Helpers;
using SharedModels.Order.Dtos;
using SharedModels.Order.Messages;

namespace ProductApi.Domain.Services;

public class ProductService : IProductService
{
    private readonly RestClient _customerClient;
    private readonly IRepository<Product> _productRepository;

    public ProductService(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
        _customerClient = new RestClient(RestConnectionHelper.GetCustomerUrl());
    }

    public Product Add(Product product)
    {
        if (product.Name == null || product.Price == 0 || product.ItemsInStock == -1 || product.ItemsReserved == -1)
            throw new ArgumentException("Product must not be null");

        return _productRepository.Add(product);
    }

    public Product Get(int id)
    {
        var product = _productRepository.Get(id);

        if (product == null)
            throw new ArgumentException("Product not found");

        return product;
    }

    public IEnumerable<Product> GetAll()
    {
        return _productRepository.GetAll();
    }

    public Product Edit(Product entity)
    {
        return _productRepository.Edit(entity);
    }

    public Product Remove(Product entity)
    {
        return _productRepository.Remove(entity);
    }

    public Dictionary<bool, OrderRejectReason> IsOrderValid(OrderCreatedMessage orderCreatedMessage)
    {
        var orderAccepted = true;
        var orderRejected = OrderRejectReason.Unknown;

        // Check if customer exists
        var customerExistRequest = new RestRequest(orderCreatedMessage.CustomerId.ToString());
        var customerExistResponse = _customerClient.GetAsync<CustomerDto>(customerExistRequest).Result;

        if (customerExistResponse is null)
        {
            orderAccepted = false;
            orderRejected = OrderRejectReason.CustomerDoesNotExist;
        }
        
        Console.WriteLine(orderAccepted ? "customer exist": "customer does not exist");

        // Check if customer has enough credit
        var customerEnoughCreditRequest = new RestRequest($"hasMinCredit/{orderCreatedMessage.CustomerId}");
        var customerEnoughCreditResponse = _customerClient.GetAsync<bool>(customerEnoughCreditRequest).Result;
        
        Console.WriteLine(customerEnoughCreditResponse);
        
        if (!customerEnoughCreditResponse)
        {
            orderAccepted = false;
            orderRejected = OrderRejectReason.CustomerCreditIsNotGoodEnough;
        }
        
        Console.WriteLine(orderAccepted ? "customer has enough credit": "customer does not have enough credit");

        // Check if customer has outstanding bills
        var customerOutstandingBillsRequest = new RestRequest($"bills/{orderCreatedMessage.CustomerId}");
        var customerOutstandingBillsResponse = _customerClient.GetAsync<bool>(customerOutstandingBillsRequest).Result;

        if (!customerOutstandingBillsResponse)
        {
            orderAccepted = false;
            orderRejected = OrderRejectReason.CustomerOutstandingBills;
        }
        
        Console.WriteLine(orderAccepted ? "customer has no outstanding bills": "customer has outstanding bills");

        // Check if product is available
        if (!IsProductAvailable(orderCreatedMessage.OrderLines))
        {
            orderAccepted = false;
            orderRejected = OrderRejectReason.InsufficientStock;
        }
        
        Console.WriteLine(orderAccepted ? "product is available": "product is not available");

        return new Dictionary<bool, OrderRejectReason>
        {
            { orderAccepted, orderRejected }
        };
    }

    public void ReserveProduct(IEnumerable<OrderLineDto> orderLines)
    {
        foreach (var orderLine in orderLines)
        {
            var product = Get(orderLine.ProductId);
            product.ItemsReserved += orderLine.Quantity;
            _productRepository.Edit(product);
        }
    }

    private bool IsProductAvailable(IEnumerable<OrderLineDto> orderLines)
    {
        foreach (var orderLine in orderLines)
        {
            var product = _productRepository.Get(orderLine.ProductId);
            if (orderLine.Quantity > product.ItemsInStock - product.ItemsReserved) return false;
        }

        return true;
    }
}