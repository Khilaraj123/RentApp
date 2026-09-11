using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RentApp.Application.Interfaces.External;
using RentApp.Domain.Common;

namespace RentApp.Infrastructure.Services.External
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DomainEventDispatcher> _logger;

        public DomainEventDispatcher(IServiceScopeFactory scopeFactory, ILogger<DomainEventDispatcher> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
        {
            foreach(var domainEvent in domainEvents)
            {
                var eventType = domainEvent.GetType();
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);

                using var scope = _scopeFactory.CreateScope();
                var handlers = scope.ServiceProvider.GetServices(handlerType);

                foreach(var handler in handlers)
                {
                    if (handler == null) continue;

                    var method = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync));
                    if(method != null)
                    {
                        try
                        {
                            await (Task)method.Invoke(handler, new object[] { domainEvent, cancellationToken })!;
                        } catch(Exception ex)
                        {
                            _logger.LogError(ex, "Error executing event handler {HandlerName} for event {EventName}", handler.GetType().Name, eventType.Name);
                        }
                    }
                }
            }
        }
    }
}
