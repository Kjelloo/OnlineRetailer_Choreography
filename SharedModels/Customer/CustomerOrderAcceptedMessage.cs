namespace SharedModels.Customer
{
    public class CustomerOrderAcceptedMessage
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
    }
}