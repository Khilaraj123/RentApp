using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Users
{
    public class RefreshToken : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string Token { get; private set; } = string.Empty;
        public string JwtId { get; private set; } = string.Empty;
        public bool IsUsed { get; private set; }
        public bool IsRevoked { get; private set; }
        public DateTime AddedDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;
        public bool IsActive => !IsRevoked && !IsExpired;

        public virtual ApplicationUser? User { get; private set; }

        private RefreshToken() { } // EF Core

        public RefreshToken(Guid userId, string token, string jwtId, DateTime addedDate, DateTime expiryDate)
        {
            UserId = userId;
            Token = token;
            JwtId = jwtId;
            AddedDate = addedDate;
            ExpiryDate = expiryDate;
            IsUsed = false;
            IsRevoked = false;
        }

        public void MarkAsUsed()
        {
            IsUsed = true;
        }

        public void Revoke()
        {
            IsRevoked = true;
        }
    }
}
