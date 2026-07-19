using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Users
{
    public class UserProfile : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? AddressLine1 { get; private set; }
        public string? AddressLine2 { get; private set; }
        public string? City { get; private set; }
        public string? State { get; private set; }
        public string? Country { get; private set; }
        public string? ZipCode { get; private set; }
        public string? Preferences { get; private set; }

        public virtual ApplicationUser? User { get; private set; }

        private UserProfile() { } // EF Core

        public UserProfile(Guid userId)
        {
            UserId = userId;
        }

        public void UpdateAddress(
            string? addressLine1, 
            string? addressLine2, 
            string? city, 
            string? state, 
            string? country, 
            string? zipCode)
        {
            AddressLine1 = addressLine1?.Trim();
            AddressLine2 = addressLine2?.Trim();
            City = city?.Trim();
            State = state?.Trim();
            Country = country?.Trim();
            ZipCode = zipCode?.Trim();
            
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePhoneNumber(string? phoneNumber)
        {
            PhoneNumber = phoneNumber?.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePreferences(string? preferences)
        {
            Preferences = preferences?.Trim();
            UpdatedAt = DateTime.UtcNow;
        }
    }
}