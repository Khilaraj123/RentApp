namespace RentApp.Domain.Enums;

public enum BookingCancellationReason
{
    None = 0,
    UserRequested = 1,
    HostRequested = 2,
    PaymentFailed = 3,
    NoShow = 4,
    Other = 5
}
