using CustomerApi.Core.Models;
using CustomerApi.Core.Services;
using CustomerApi.Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using SharedModels;
using SharedModels.Customer;

namespace CustomerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly IConverter<Customer, CustomerDto> _converter;

        public CustomerController( IConverter<Customer, CustomerDto> converter, ICustomerService service)
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
                return BadRequest("Customer is missing required fields");
            }
        }
        
        [HttpGet("credit/{id}")]
        public ActionResult<bool> GetCustomerSufficientCredit(int id)
        {
            try
            {
                var credit = _service.SufficientCredit(id);
                
                return Ok(credit);
            }
            catch (Exception)
            {
                return NotFound("Customer not found");
            }
        }
        
        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            try
            {
                var customer = _service.Find(id);
                
                return Ok(customer);
            }
            catch (Exception)
            {
                return NotFound("Customer not found");
            }
        }
        
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            return Ok(_service.FindAll());
        }
    }
}
