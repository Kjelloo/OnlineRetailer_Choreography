using SharedModels.Order.Models;

namespace SharedModels.Customer.Messages
{
    public class CustomerOrderRejectedMessage
    {
        public Order.Models.Order Order { get; set; }
        public int CustomerId { get; set; }
        public OrderRejectReason OrderRejectReason { get; set; }
    }
}