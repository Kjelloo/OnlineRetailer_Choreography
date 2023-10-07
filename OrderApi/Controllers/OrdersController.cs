using Microsoft.AspNetCore.Mvc;
using OrderApi.Core.Services;
using OrderApi.Domain.Repositories;
using OrderApi.Infrastructure.Messages;
using SharedModels;
using SharedModels.Order;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        IOrderRepository repository;
        IMessagePublisher messagePublisher;
        private readonly IOrderService _service;

        public OrdersController(IRepository<Order> repos,
            IMessagePublisher publisher,
            IOrderService service)
        {
            repository = repos as IOrderRepository;
            messagePublisher = publisher;
            _service = service;
        }
        
        [HttpGet("customerOrders/{customerId}")]
        public IActionResult GetCustomerOrders(int customerId)
        {
            var orders = repository.GetByCustomer(customerId);
            
            if (orders == null)
            {
                return NotFound();
            }
            
            return Ok(orders);
        }

        // GET orders
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return repository.GetAll();
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public IActionResult Get(int id)
        {
            var item = repository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            
            return new ObjectResult(item);
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
                var newOrder = _service.Create(order);

                // Return a non-binding order confirmation immediately.
                // (an email will be sent when the order status becomes complete)
                return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        // PUT orders/5/cancel
        // This action method cancels an order and publishes an OrderStatusChangedMessage
        // with topic set to "cancelled".
        [HttpPut("{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            throw new NotImplementedException();

            // Add code to implement this method.
        }

        // PUT orders/5/ship
        // This action method ships an order and publishes an OrderStatusChangedMessage.
        // with topic set to "shipped".
        [HttpPut("{id}/ship")]
        public IActionResult Ship(int id)
        {
            throw new NotImplementedException();

            // Add code to implement this method.
        }

        // PUT orders/5/pay
        // This action method marks an order as paid and publishes a CreditStandingChangedMessage
        // (which have not yet been implemented), if the credit standing changes.
        [HttpPut("{id}/pay")]
        public IActionResult Pay(int id)
        {
            throw new NotImplementedException();

            // Add code to implement this method.
        }

    }
}
