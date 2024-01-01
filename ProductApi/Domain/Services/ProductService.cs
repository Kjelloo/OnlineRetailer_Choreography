using ProductApi.Core.Models;
using ProductApi.Core.Proxies;
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
    private readonly IRepository<Product> _productRepository;
    private readonly ICustomerProxyService _customerProxyService;

    public ProductService(IRepository<Product> productRepository, ICustomerProxyService customerProxyService)
    {
        _productRepository = productRepository;
        _customerProxyService = customerProxyService;
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
        var customerExistResponse = _customerProxyService.GetCustomer(orderCreatedMessage.CustomerId).Result;
        
        if (customerExistResponse is null)
        {
            orderAccepted = false;
            orderRejected = OrderRejectReason.CustomerDoesNotExist;
        }

        // Check if customer has enough credit
        var customerEnoughCreditResponse = _customerProxyService.HasMinCredit(orderCreatedMessage.CustomerId).Result;
        if (!customerEnoughCreditResponse)
        {
            orderAccepted = false;
            orderRejected = OrderRejectReason.CustomerCreditIsNotGoodEnough;
        }

        // Check if customer has outstanding bills
        var customerOutstandingBillsResponse = _customerProxyService.HasOutstandingBills(orderCreatedMessage.CustomerId).Result;
        if (!customerOutstandingBillsResponse)
        {
            orderAccepted = false;
            orderRejected = OrderRejectReason.CustomerOutstandingBills;
        }

        // Check if product is available
        if (!IsProductAvailable(orderCreatedMessage.OrderLines))
        {
            orderAccepted = false;
            orderRejected = OrderRejectReason.InsufficientStock;
        }
        
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