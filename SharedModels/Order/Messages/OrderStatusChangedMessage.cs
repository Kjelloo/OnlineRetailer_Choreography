using SharedModels.Order.Dtos;

namespace SharedModels.Order.Messages
{
    public class OrderStatusChangedMessage
    {
        public OrderDto Order { get; set; }
        public int CustomerId { get; set; }
        public OrderStatusDto OrderStatus { get; set; }
    }
}