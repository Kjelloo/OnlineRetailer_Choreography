namespace SharedModels.Customer.Messages
{
    public class CustomerCreditStandingChangedMessage
    {
        public int CustomerId { get; set; }
        public int Credit { get; set; }
    }
}