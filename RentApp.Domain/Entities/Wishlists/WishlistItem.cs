using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Wishlists
{
    public class WishlistItem : BaseEntity
    {
        public Guid WishlistId { get; private set; }
        public Guid ListingId { get; private set; }

        private WishlistItem() { } // EF Core

        public WishlistItem(Guid wishlistId, Guid listingId)
        {
            WishlistId = wishlistId;
            ListingId = listingId;
        }
    }
}
