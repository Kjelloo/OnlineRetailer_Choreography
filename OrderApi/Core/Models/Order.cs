﻿namespace OrderApi.Core.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime? Date { get; set; }
    public int CustomerId { get; set; }
    public OrderStatus Status { get; set; }
    public IEnumerable<OrderLine> OrderLines { get; set; }

    public override string ToString()
    {
        return $"Order id: {Id}, Date: {Date}, customerId: {CustomerId}, Status: {Status}";
    }
}