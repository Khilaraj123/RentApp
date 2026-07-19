using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class CategoryRenamedEvent : DomainEvent
    {
        public Guid CategoryId { get; }

        public CategoryRenamedEvent(Guid categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
