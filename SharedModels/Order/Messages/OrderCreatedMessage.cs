using System.Collections.Generic;
using SharedModels.Order.Models;

namespace SharedModels.Order.Messages
{
    public class OrderCreatedMessage
    {
        public int? CustomerId { get; set; }
        public int OrderId { get; set; }
        public IList<OrderLine> OrderLines { get; set; }
    }
}
