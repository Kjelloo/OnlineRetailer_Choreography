﻿namespace SharedModels.Order.Dtos
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