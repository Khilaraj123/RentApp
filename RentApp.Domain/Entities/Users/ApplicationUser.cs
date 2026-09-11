using Microsoft.AspNetCore.Identity;
using RentApp.Domain.Common;
using RentApp.Domain.Entities.Bookings;
using RentApp.Domain.Entities.Listings;
using RentApp.Domain.Entities.Reviews;
using RentApp.Domain.Entities.Wishlists;

namespace RentApp.Domain.Entities.Users
{
    public class ApplicationUser : IdentityUser<Guid>, IHasDomainEvents
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? ProfilePictureUrl { get; private set; }
        public DateOnly? DateOfBirth { get; private set; }
        public string? Bio { get; private set; }

        public bool IsVerified { get; private set; }
        public bool IsIdentityVerified { get; private set; }
        public bool IsBlocked { get; private set; }
        public bool IsDeleted { get; private set; }
        public bool IsEnabled => !IsBlocked && !IsDeleted;

        public DateTime? DeletedAtUtc { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? UpdatedAtUtc { get; private set; }
        public DateTime? LastLoginAtUtc { get; private set; }
        
        public decimal AverageRating { get; private set; }
        public int ReviewCount { get; private set; }
        public int CompletedRentalCount { get; private set; }
        public int ActiveListingCount { get; private set; }
        public double ResponseRate { get; private set; }

        public TimeSpan? AverageResponseTime { get; private set; }

        public virtual UserProfile? Profile { get; private set; }
        public virtual ICollection<Listing> Listings { get; private set; }
        public virtual ICollection<Booking> CustomerBookings { get; private set; }
        public virtual ICollection<Booking> OwnerBookings { get; private set; }
        public virtual ICollection<Review> ReviewsWritten { get; private set; }
        public virtual ICollection<Review> ReviewsReceived { get; private set; }
        public virtual ICollection<Wishlist> WishlistItems { get; private set; }
        
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents =>
      _domainEvents.AsReadOnly();
        public ApplicationUser()
        {
            Listings = new List<Listing>();
            CustomerBookings = new List<Booking>();
            OwnerBookings = new List<Booking>();

            ReviewsWritten = new List<Review>();
            ReviewsReceived = new List<Review>();

            WishlistItems = new List<Wishlist>();

            CreatedAtUtc = DateTime.UtcNow;
        }

        public ApplicationUser(
            string firstName,
            string lastName,
            string email)
            : this()
        {
            Id = Guid.NewGuid();

            FirstName = firstName;
            LastName = lastName;

            Email = email;
            UserName = email;

            EmailConfirmed = false;

            IsVerified = false;
            IsIdentityVerified = false;
            IsBlocked = false;
            IsDeleted = false;
        }


        public string FullName => $"{FirstName} {LastName}".Trim();

        public void UpdateProfile(
            string firstName,
            string lastName,
            DateOnly? dateOfBirth,
            string? bio)
        {
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            DateOfBirth = dateOfBirth;
            Bio = bio?.Trim();

            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void ChangeProfilePicture(string? pictureUrl)
        {
            ProfilePictureUrl = pictureUrl;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void UpdateBio(string? bio)
        {
            Bio = bio?.Trim();
            UpdatedAtUtc = DateTime.UtcNow;
        }


        public void VerifyEmail()
        {
            if (EmailConfirmed)
                return;

            EmailConfirmed = true;
            IsVerified = true;

            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void VerifyIdentity()
        {
            if (IsIdentityVerified)
                return;

            IsIdentityVerified = true;
            UpdatedAtUtc = DateTime.UtcNow;
        }


        public void Block()
        {
            if (IsBlocked)
                return;

            IsBlocked = true;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Unblock()
        {
            if (!IsBlocked)
                return;

            IsBlocked = false;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            if (IsDeleted)
                return;

            IsDeleted = true;
            DeletedAtUtc = DateTime.UtcNow;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void Restore()
        {
            if (!IsDeleted)
                return;

            IsDeleted = false;
            DeletedAtUtc = null;

            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void RecordLogin()
        {
            LastLoginAtUtc = DateTime.UtcNow;
        }

        public void UpdateRating(decimal averageRating, int reviewCount)
        {
            if (averageRating < 0)
                averageRating = 0;

            if (averageRating > 5)
                averageRating = 5;

            AverageRating = averageRating;
            ReviewCount = reviewCount;

            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void IncrementCompletedRentals()
        {
            CompletedRentalCount++;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void IncrementActiveListings()
        {
            ActiveListingCount++;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void DecrementActiveListings()
        {
            if (ActiveListingCount > 0)
                ActiveListingCount--;

            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void UpdateResponseMetrics(
            double responseRate,
            TimeSpan averageResponseTime)
        {
            ResponseRate = Math.Clamp(responseRate, 0, 100);
            AverageResponseTime = averageResponseTime;

            UpdatedAtUtc = DateTime.UtcNow;
        }





        public void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public void RemoveDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }
    }
}