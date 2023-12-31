namespace SharedModels.Order.Dtos
{
    public enum OrderStatusDto
    {
        Tentative = 0,
        WaitingToBePaid = 1,
        WaitingToBeShipped = 2,
        Shipped = 3,
        Cancelled = 4,
        Completed = 5
    }
}