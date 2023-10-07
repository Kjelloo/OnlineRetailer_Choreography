using SharedModels.Order.Dtos;

namespace SharedModels.Customer.Messages
{
    public class CustomerOrderUpdatedMessage
    {
        public int CustomerId { get; set; }
        public OrderDto Order { get; set; }
    }
}