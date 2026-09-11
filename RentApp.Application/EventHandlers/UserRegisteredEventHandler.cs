using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RentApp.Application.Interfaces.External;
using RentApp.Domain.DomainEvents.User;

namespace RentApp.Application.EventHandlers
{
    public class UserRegisteredEventHandler : IDomainEventHandler<UserRegisteredEvent>
    {
        private readonly ILogger<UserRegisteredEventHandler> _logger;

        public UserRegisteredEventHandler(ILogger<UserRegisteredEventHandler> logger)
        {
            _logger = logger;
        }

        public Task HandleAsync(UserRegisteredEvent domainEvent, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User registered event received for user {UserId} ({FullName}, {Email})",
                domainEvent.UserId, domainEvent.FullName, domainEvent.Email);

            return Task.CompletedTask;
        }
    }
}
