namespace SharedModels.Order.Dtos
{
    public enum OrderStatusDto
    {
        Tentative,
        WaitingToBeShipped,
        Shipped,
        Cancelled,
        Completed
    }
}