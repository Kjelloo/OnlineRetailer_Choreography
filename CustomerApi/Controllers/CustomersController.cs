using CustomerApi.Core.Models;
using CustomerApi.Core.Services;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedModels.Customer;

namespace CustomerApi.Controllers;

[Route("[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _service;
    private readonly IConverter<Customer, CustomerDto> _converter;

    public CustomersController( IConverter<Customer, CustomerDto> converter, ICustomerService service)
    {
        _converter = converter;
        _service = service;
    }

    [HttpPost]
    public IActionResult Post([FromBody] CustomerDto customerDto)
    {
        try
        {
            var newCustomer = _service.Add(_converter.Convert(customerDto));
            
            return Created($"Created customer {newCustomer.Name}", newCustomer);
        }
        catch (ArgumentException e)
        {
            return BadRequest("Customer is missing required fields: " + e.Message);
        }
    }
    
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            var customer = _service.Get(id);
            
            var customerDto = _converter.Convert(customer);
            
            return Ok(customerDto);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet("orders/{id}")]
    public IActionResult GetOrdersForCustomer(int id)
    {
        try
        {
            var ordersDto = _service.GetCustomerOrders(id);
            
            return Ok(ordersDto);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_service.GetAll().Select(c => _converter.Convert(c)));
    }
    
    [HttpGet("credit/{id}")]
    public IActionResult GetCustomerSufficientCredit(int id)
    {
        try
        {
            var credit = _service.SufficientCredit(id);
            
            return Ok(credit);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet("bills/{id}")]
    public IActionResult GetCustomerOutstandingBills(int id)
    {
        try
        {
            return Ok(_service.OutstandingBills(id));
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
}
