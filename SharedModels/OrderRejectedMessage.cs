namespace SharedModels
{
    public class OrderRejectedMessage
    {
        public int OrderId { get; set; }
        public OrderRejectReason Reason { get; set; }
    }
}
