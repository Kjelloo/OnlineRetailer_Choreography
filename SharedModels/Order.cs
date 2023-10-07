using System;
using System.Collections.Generic;

namespace SharedModels
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int CustomerId { get; set; }
        public OrderStatus Status { get; set; }
        public IList<OrderLine> OrderLines { get; set; }

        public enum OrderStatus
        {
            tentative,
            cancelled,
            completed,
            shipped,
            paid
        }
        
        public override string ToString()
        {
            return $"Order id: {Id}, Date: {Date}, customerId: {CustomerId}, Status: {Status}";
        }
    }

    public class OrderLine
    {
        public int id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public override string ToString()
        {
            return $"OrderLine id: {id}, OrderId: {OrderId}, ProductId: {ProductId}, Quantity: {Quantity}";
        }
    }
}
