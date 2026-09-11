namespace RentApp.Domain.Common
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
        void AddDomainEvent(IDomainEvent domainEvent);
        void ClearDomainEvents();
        void RemoveDomainEvent(IDomainEvent domainEvent);
    }
}
