namespace OrderApi.Core.Models;

public enum OrderStatus
{
    Tentative = 0,
    WaitingToBePaid = 1,
    WaitingToBeShipped = 2,
    Shipped = 3,
    Cancelled = 4,
    Completed = 5
}