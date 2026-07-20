using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Wishlist
{
    public class WishlistItemAddedEvent : DomainEvent
    {
        public Guid WishlistId { get; }
        public Guid ListingId { get; }

        public WishlistItemAddedEvent(Guid wishlistId, Guid listingId)
        {
            WishlistId = wishlistId;
            ListingId = listingId;
        }
    }
}
