namespace RentApp.Domain.Enums;

public enum AgreementStatus
{
    Draft = 1,
    Generated = 2,
    WaitingForOwner = 3,
    WaitingForRenter = 4,
    FullySigned = 5,
    Cancelled = 6,
    Expired = 7
}
