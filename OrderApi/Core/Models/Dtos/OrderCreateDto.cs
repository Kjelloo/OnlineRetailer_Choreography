namespace OrderApi.Core.Models.Dtos;

public class OrderCreateDto
{
    public int CustomerId { get; set; }
    public IEnumerable<OrderLineCreateDto> OrderLines { get; set; }
}