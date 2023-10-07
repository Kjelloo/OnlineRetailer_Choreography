using System.Collections.Generic;
using SharedModels.Order.Dtos;

namespace SharedModels.Order.Messages
{
    public class OrderCreatedMessage
    {
        public int? CustomerId { get; set; }
        public int OrderId { get; set; }
        public IList<OrderLineDto> OrderLines { get; set; }
    }
}