namespace SharedModels.Customer.Messages
{
    public class CustomerOrderUpdatedMessage
    {
        public int CustomerId { get; set; }
        public Order.Models.Order Order { get; set; }
    }
}