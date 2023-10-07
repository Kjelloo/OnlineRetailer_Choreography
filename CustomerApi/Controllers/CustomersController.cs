using CustomerApi.Core.Models;
using CustomerApi.Core.Services;
using Microsoft.AspNetCore.Mvc;
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
    public ActionResult Post([FromBody] CustomerDto customerDto)
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
    public ActionResult<Customer> Get(int id)
    {
        try
        {
            var customer = _service.Get(id);
            
            return Ok(customer);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<Customer>> Get()
    {
        return Ok(_service.GetAll());
    }
    
    [HttpGet("credit/{id}")]
    public ActionResult<bool> GetCustomerSufficientCredit(int id)
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
    public ActionResult<bool> GetCustomerOutstandingBills(int id)
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
