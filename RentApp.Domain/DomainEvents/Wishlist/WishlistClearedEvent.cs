using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Wishlist
{
    public class WishlistClearedEvent : DomainEvent
    {
        public Guid WishlistId { get; }

        public WishlistClearedEvent(Guid wishlistId)
        {
            WishlistId = wishlistId;
        }
    }
}
