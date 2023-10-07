namespace OrderApi.Core.Models;

public class OrderLine
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public override string ToString()
    {
        return $"OrderLine id: {Id}, OrderId: {OrderId}, ProductId: {ProductId}, Quantity: {Quantity}";
    }
}