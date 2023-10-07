using SharedModels.Order.Models;

namespace SharedModels.Order.Messages
{
    public class OrderRejectedMessage
    {
        public int OrderId { get; set; }
        public OrderRejectReason Reason { get; set; }
    }
}
