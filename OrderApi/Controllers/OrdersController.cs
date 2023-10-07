using Microsoft.AspNetCore.Mvc;
using OrderApi.Core.Models;
using OrderApi.Core.Services;
using SharedModels;
using SharedModels.Order.Dtos;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IConverter<Order, OrderDto> _converter;

        public OrdersController(IOrderService service, IConverter<Order, OrderDto> converter)
        {
            _service = service;
            _converter = converter;
        }
        
        // POST orders
        [HttpPost]
        public IActionResult Post([FromBody]Order order)
        {
            if (order == null)
            {
                return BadRequest();
            }
            
            try
            {
                var newOrder = _service.Add(order);

                // Return a non-binding order confirmation immediately.
                // (an email will be sent when the order status becomes complete)
                return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // GET orders
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetAll().Select(o => _converter.Convert(o)));
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = _service.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            
            var orderDto = _converter.Convert(item);
            
            return Ok(orderDto);
        }
        
        [HttpGet("customerOrders/{customerId}")]
        public IActionResult GetCustomerOrders(int customerId)
        {
            var orders = _service.GetByCustomer(customerId);
            
            if (orders == null)
            {
                return NotFound();
            }
            
            var ordersDto = orders.Select(o => _converter.Convert(o));
            
            return Ok(ordersDto);
        }
        
        // PUT orders/5/cancel
        // This action method cancels an order and publishes an OrderStatusChangedMessage
        // with topic set to "cancelled".
        [HttpPut("cancel/{id}")]
        public IActionResult Cancel(int id)
        {
            throw new NotImplementedException();

            // Add code to implement this method.
        }

        // PUT orders/5/ship
        // This action method ships an order and publishes an OrderStatusChangedMessage.
        // with topic set to "shipped".
        [HttpPut("ship/{id}")]
        public IActionResult Ship(int id)
        {
            throw new NotImplementedException();
            // Add code to implement this method.
        }

        // PUT orders/5/pay
        // This action method marks an order as paid and publishes a CreditStandingChangedMessage
        // (which have not yet been implemented), if the credit standing changes.
        [HttpPut("/pay{id}")]
        public IActionResult Pay(int id)
        {
            throw new NotImplementedException();
            // Add code to implement this method.
        }

    }
}
