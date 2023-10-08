using System;
using System.Collections.Generic;

namespace SharedModels.Order.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int CustomerId { get; set; }
        public OrderStatusDto Status { get; set; }
        public IEnumerable<OrderLineDto> OrderLines { get; set; }

        public override string ToString()
        {
            return $"Order id: {Id}, Date: {Date}, customerId: {CustomerId}, Status: {Status}";
        }
    }
}