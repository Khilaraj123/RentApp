using RentApp.Domain.Common;

namespace RentApp.Application.Interfaces.External
{
    public interface IDomainEventHandler
    {
        Task HandleAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
    }
}
