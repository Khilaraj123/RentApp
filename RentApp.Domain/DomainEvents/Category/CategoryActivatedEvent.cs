using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class CategoryActivatedEvent : DomainEvent
    {
        public Guid CategoryId { get; }

        public CategoryActivatedEvent(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
