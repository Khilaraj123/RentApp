using System;
using System.Collections.Generic;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Messaging
{
    public class Conversation : AuditableEntity
    {
        public Guid ListingId { get; private set; }
        public Guid RenterId { get; private set; }
        public Guid OwnerId { get; private set; }

        private readonly List<Message> _messages = new();
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();

        private Conversation() { } // EF Core

        public Conversation(Guid listingId, Guid renterId, Guid ownerId)
        {
            ListingId = listingId;
            RenterId = renterId;
            OwnerId = ownerId;
        }

        public void AddMessage(Message message)
        {
            _messages.Add(message);
        }
    }
}
