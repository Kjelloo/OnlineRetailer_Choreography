namespace OrderApi.Core.Models;

public enum OrderStatus
{
    Tentative,
    WaitingToBeShipped,
    Shipped,
    Cancelled,
    Completed
}