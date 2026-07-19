using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.DomainEvents;

namespace RentApp.Domain.Entities.Messaging
{
    public class Conversation : AuditableEntity
    {
        public Guid? ListingId { get; private set; }
        public Guid? BookingId { get; private set; }
        
        public Guid OwnerId { get; private set; }
        public Guid RenterId { get; private set; }

        public ConversationStatus Status { get; private set; }

        public Guid? LastMessageId { get; private set; }
        public string? LastMessagePreview { get; private set; }
        public DateTime? LastMessageAtUtc { get; private set; }

        public int OwnerUnreadCount { get; private set; }
        public int RenterUnreadCount { get; private set; }

        public DateTime? OwnerLastReadAtUtc { get; private set; }
        public DateTime? RenterLastReadAtUtc { get; private set; }

        private Conversation() { } // EF Core

        public Conversation(Guid? listingId, Guid? bookingId, Guid ownerId, Guid renterId)
        {
            if (listingId == null && bookingId == null)
            {
                throw new ArgumentException("A conversation must be linked to either a Listing or a Booking.");
            }

            if (ownerId == Guid.Empty) throw new ArgumentException("OwnerId cannot be empty.", nameof(ownerId));
            if (renterId == Guid.Empty) throw new ArgumentException("RenterId cannot be empty.", nameof(renterId));
            
            if (ownerId == renterId)
            {
                throw new ArgumentException("Owner and Renter cannot be the same user.");
            }

            ListingId = listingId;
            BookingId = bookingId;
            OwnerId = ownerId;
            RenterId = renterId;
            
            Status = ConversationStatus.Active;
            
            AddDomainEvent(new ConversationStartedEvent(Id));
        }

        public void AddMessage(Message message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            
            if (Status == ConversationStatus.Closed)
            {
                throw new InvalidOperationException("Cannot add a message to a closed conversation.");
            }

            if (message.SenderId != OwnerId && message.SenderId != RenterId)
            {
                throw new ArgumentException("Message sender is not a participant of this conversation.");
            }

            LastMessageId = message.Id;
            LastMessagePreview = message.Content;
            LastMessageAtUtc = message.SentAt;

            if (message.SenderId == OwnerId)
            {
                RenterUnreadCount++;
            }
            else if (message.SenderId == RenterId)
            {
                OwnerUnreadCount++;
            }

            AddDomainEvent(new MessageSentEvent(Id, message.Id));
        }

        public void MarkAsRead(Guid userId)
        {
            if (userId == OwnerId)
            {
                OwnerUnreadCount = 0;
                OwnerLastReadAtUtc = DateTime.UtcNow;
            }
            else if (userId == RenterId)
            {
                RenterUnreadCount = 0;
                RenterLastReadAtUtc = DateTime.UtcNow;
            }
            else
            {
                throw new ArgumentException("User is not a participant of this conversation.");
            }
        }

        public void Archive()
        {
            if (Status == ConversationStatus.Closed)
            {
                throw new InvalidOperationException("Cannot archive a closed conversation.");
            }
            
            Status = ConversationStatus.Archived;
        }

        public void Reopen()
        {
            if (Status != ConversationStatus.Archived && Status != ConversationStatus.Closed)
            {
                throw new InvalidOperationException($"Cannot reopen a conversation from status {Status}.");
            }
            
            Status = ConversationStatus.Active;
        }
    }
}
