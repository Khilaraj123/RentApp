using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Wishlist
{
    public class WishlistItemRemovedEvent : DomainEvent
    {
        public Guid WishlistId { get; }
        public Guid ListingId { get; }

        public WishlistItemRemovedEvent(Guid wishlistId, Guid listingId)
        {
            WishlistId = wishlistId;
            ListingId = listingId;
        }
    }
}
