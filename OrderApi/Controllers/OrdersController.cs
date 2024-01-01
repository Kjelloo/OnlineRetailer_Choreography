using Microsoft.AspNetCore.Mvc;
using OrderApi.Core.Models;
using OrderApi.Core.Models.Dtos;
using OrderApi.Core.Services;
using SharedModels;
using SharedModels.Order.Dtos;

namespace OrderApi.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IConverter<Order, OrderDto> _converter;
    private readonly IOrderService _service;

    public OrdersController(IOrderService service, IConverter<Order, OrderDto> converter)
    {
        _service = service;
        _converter = converter;
    }

    // POST orders
    [HttpPost]
    public IActionResult Post([FromBody] OrderCreateDto orderCreateDto)
    {
        if (orderCreateDto == null) return BadRequest();

        var orderLines = orderCreateDto.OrderLines.Select(orderLine => 
            new OrderLine { ProductId = orderLine.ProductId, Quantity = orderLine.Quantity }).ToList();

        var order = new Order
        {
            CustomerId = orderCreateDto.CustomerId,
            OrderLines = orderLines
        };
        
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
    [HttpGet(nameof(Get))]
    public ActionResult<IEnumerable<OrderDto>> Get()
    {
        var orders = _service.GetAll();
        return Ok(orders.Select(order => _converter.Convert(order)));
    }

    // GET orders/5
    [HttpGet("{id}", Name = "GetOrder")]
    public IActionResult Get(int id)
    {
        var item = _service.Get(id);
        if (item == null) return NotFound();

        var orderDto = _converter.Convert(item);

        return Ok(orderDto);
    }

    [HttpGet("customerOrders/{customerId}")]
    public ActionResult<IEnumerable<OrderDto>> GetCustomerOrders(int customerId)
    {
        var orders = _service.GetByCustomer(customerId);

        if (orders == null) return NotFound();

        var ordersDto = orders.Select(o => _converter.Convert(o));

        return Ok(ordersDto);
    }

    // PUT orders/cancel/5
    // This action method cancels an order and publishes an OrderStatusChangedMessage
    // with topic set to "cancelled".
    [HttpPut("cancel/{id}")]
    public IActionResult Cancel(int id)
    {
        try
        {
            return Ok("Order cancelled: " + _service.Cancel(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // PUT orders/ship/5
    // This action method ships an order and publishes an OrderStatusChangedMessage.
    // with topic set to "shipped".
    [HttpPut("ship/{id}")]
    public IActionResult Ship(int id)
    {
        try
        {
            return Ok("Order shipped: " + _service.Ship(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // PUT orders/pay/5
    // This action method marks an order as paid and publishes a CreditStandingChangedMessage
    // (which have not yet been implemented), if the credit standing changes.
    [HttpPut("pay/{id}")]
    public IActionResult Pay(int id)
    {
        try
        {
            return Ok("Order paid: " + _service.Pay(id));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}