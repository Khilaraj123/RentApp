using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class CategoryDeactivatedEvent : DomainEvent
    {
        public Guid CategoryId { get; }

        public CategoryDeactivatedEvent(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
