using Microsoft.AspNetCore.Identity;
using RentApp.Domain.Entities.Agreements;
using RentApp.Domain.Entities.Bookings;
using RentApp.Domain.Entities.Disputes;
using RentApp.Domain.Entities.Listings;
using RentApp.Domain.Entities.Messaging;
using RentApp.Domain.Entities.Notifications;
using RentApp.Domain.Entities.Payments;
using RentApp.Domain.Entities.Reports;
using RentApp.Domain.Entities.Reviews;
using RentApp.Domain.Entities.Wishlists;

namespace RentApp.Domain.Entities.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string? ProfilePictureUrl { get; private set; }
        public DateOnly? DateOfBirth { get; private set; }
        public string? Bio { get; private set; }

        public bool IsVerified { get; private set; }
        public bool IsIdentityVerified { get; private set; }
        public bool IsBlocked { get; private set; }
        public bool IsDeleted { get; private set; }

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

        public bool EmailNotificationsEnabled { get; private set; } = true;
        public bool PushNotificationsEnabled { get; private set; } = true;
        public bool SmsNotificationsEnabled { get; private set; }


        public virtual UserProfile? Profile { get; private set; }
        public virtual ICollection<Listing> Listings { get; private set; }
        public virtual ICollection<Booking> CustomerBookings { get; private set; }
        public virtual ICollection<Booking> OwnerBookings { get; private set; }
        public virtual ICollection<Review> ReviewsWritten { get; private set; }
        public virtual ICollection<Review> ReviewsReceived { get; private set; }
        public virtual ICollection<Wishlist> WishlistItems { get; private set; }
        public virtual ICollection<Payment> Payments { get; private set; }
        public virtual ICollection<Refund> Refunds { get; private set; }
        public virtual ICollection<Conversation> ConversationsAsOwner { get; private set; }
        public virtual ICollection<Conversation> ConversationsAsCustomer { get; private set; }
        public virtual ICollection<Message> Messages { get; private set; }
        public virtual ICollection<Notification> Notifications { get; private set; }
        public virtual ICollection<Dispute> DisputesCreated { get; private set; }
        public virtual ICollection<Report> ReportsCreated { get; private set; }
        public virtual ICollection<RentalAgreement> RentalAgreements { get; private set; }

        private ApplicationUser()
        {
            Listings = new List<Listing>();
            CustomerBookings = new List<Booking>();
            OwnerBookings = new List<Booking>();

            ReviewsWritten = new List<Review>();
            ReviewsReceived = new List<Review>();

            WishlistItems = new List<Wishlist>();

            Payments = new List<Payment>();
            Refunds = new List<Refund>();

            ConversationsAsOwner = new List<Conversation>();
            ConversationsAsCustomer = new List<Conversation>();

            Messages = new List<Message>();

            Notifications = new List<Notification>();

            DisputesCreated = new List<Dispute>();

            ReportsCreated = new List<Report>();

            RentalAgreements = new List<RentalAgreement>();

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

        #endregion


        #region Statistics

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



        public void EnableEmailNotifications()
        {
            EmailNotificationsEnabled = true;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void DisableEmailNotifications()
        {
            EmailNotificationsEnabled = false;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void EnablePushNotifications()
        {
            PushNotificationsEnabled = true;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void DisablePushNotifications()
        {
            PushNotificationsEnabled = false;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void EnableSmsNotifications()
        {
            SmsNotificationsEnabled = true;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public void DisableSmsNotifications()
        {
            SmsNotificationsEnabled = false;
            UpdatedAtUtc = DateTime.UtcNow;
        }


    }
}