using SharedModels.Order;

namespace SharedModels.Customer
{
    public class CustomerOrderRejectedMessage
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public OrderRejectReason OrderRejectReason { get; set; }
    }
}