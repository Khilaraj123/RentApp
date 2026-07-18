using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Listings
{
    public class ListingImage : BaseEntity
    {
        public Guid ListingId { get; private set; }
        public string ImageUrl { get; private set; } = string.Empty;
        public bool IsPrimary { get; private set; }
        public int Order { get; private set; }

        private ListingImage() { } // EF Core

        public ListingImage(Guid listingId, string imageUrl, bool isPrimary, int order)
        {
            ListingId = listingId;
            ImageUrl = imageUrl;
            IsPrimary = isPrimary;
            Order = order;
        }

        public void SetPrimary(bool isPrimary)
        {
            IsPrimary = isPrimary;
        }
    }
}
