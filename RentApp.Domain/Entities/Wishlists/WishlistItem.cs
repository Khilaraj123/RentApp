using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Wishlists
{
    public class WishlistItem : AuditableEntity
    {
        public Guid WishlistId { get; private set; }
        public Guid ListingId { get; private set; }

        internal WishlistItem() { } // EF Core

        internal WishlistItem(Guid wishlistId, Guid listingId)
        {
            WishlistId = wishlistId;
            ListingId = listingId;
        }
    }
}
