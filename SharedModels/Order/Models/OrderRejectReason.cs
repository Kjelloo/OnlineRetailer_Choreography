namespace SharedModels.Order.Models
{
    public enum OrderRejectReason
    {
        Unknown,
        CustomerDoesNotExist,
        CustomerCreditIsNotGoodEnough,
        CustomerOutstandingBills,
        InsufficientStock
    }
}