namespace SharedModels
{
    public class CustomerExistsMessage
    {
        public int CustomerId { get; set; }
        public bool Exists { get; set; }
    }
}