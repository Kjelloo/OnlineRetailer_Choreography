using SharedModels.Order.Dtos;

namespace SharedModels.Customer.Messages
{
    public class CustomerOrderRejectedMessage
    {
        public OrderDto Order { get; set; }
        public int CustomerId { get; set; }
        public OrderRejectReason OrderRejectReason { get; set; }
    }
}