using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Users
{
    public class RefreshToken : AggregateRoot
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string TokenHash { get; set; } = string.Empty;
        public Guid TokenFamilyId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public string? CreatedByIp { get; set; }

        public DateTime AddedDate => CreatedAt;
        public DateTime ExpiryDate => ExpiresAt;

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired && !IsUsed;

        public virtual ApplicationUser? User { get; set; }

        public RefreshToken()
        {
            Id = Guid.NewGuid();
        }

        public RefreshToken(Guid userId, string token, string tokenHash, Guid tokenFamilyId, DateTime expiresAt, string? createdByIp)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = token;
            TokenHash = tokenHash;
            TokenFamilyId = tokenFamilyId;
            ExpiresAt = expiresAt;
            CreatedByIp = createdByIp;
            CreatedAt = DateTime.UtcNow;
        }

        public RefreshToken(Guid userId, string token, DateTime expiresAt)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = token;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        public RefreshToken(Guid userId, string token, string jwtId, DateTime addedDate, DateTime expiryDate)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Token = token;
            CreatedAt = addedDate;
            ExpiresAt = expiryDate;
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
