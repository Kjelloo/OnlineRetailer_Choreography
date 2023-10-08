namespace OrderApi.Core.Models.Dtos;

public class OrderLineCreateDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}