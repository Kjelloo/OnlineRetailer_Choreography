namespace OrderApi.Core.Models;

public enum OrderStatus
{
    Tentative,
    Completed,
    WaitingToBeShipped,
    Shipped
}