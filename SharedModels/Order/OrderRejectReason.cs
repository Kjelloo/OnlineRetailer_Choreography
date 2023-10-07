namespace SharedModels.Order
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