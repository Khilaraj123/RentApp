using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Reviews
{
    public class ReviewImage : BaseEntity
    {
        public Guid ReviewId { get; private set; }
        public string ImageUrl { get; private set; } = string.Empty;

        private ReviewImage() { }

        public ReviewImage(Guid reviewId, string imageUrl)
        {
            ReviewId = reviewId;
            ImageUrl = imageUrl;
        }
    }
}
