using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Category
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
