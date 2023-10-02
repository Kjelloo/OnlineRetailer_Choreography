using CustomerApi.Core.Models;
using CustomerApi.Core.Services;
using Microsoft.AspNetCore.Mvc;
using SharedModels;

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
        
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> Get()
        {
            return Ok(_service.Get());
        }
    }
}
