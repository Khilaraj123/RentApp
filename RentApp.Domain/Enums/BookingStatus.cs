namespace RentApp.Domain.Enums;

public enum BookingStatus
{
    Pending = 1,
    WaitingPayment = 2,
    Confirmed = 3,
    PickedUp = 4,
    Active = 5,
    Returned = 6,
    Completed = 7,
    Cancelled = 8,
    Rejected = 9,
    Expired = 10,
    Refunded = 11
}
