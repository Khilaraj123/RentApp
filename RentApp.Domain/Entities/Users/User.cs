using System;
using System.Collections.Generic;
using RentApp.Domain.Common;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Users
{
    public class User : SoftDeleteEntity
    {
        public string Email { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string? Phone { get; private set; }
        public string? Bio { get; private set; }
        public string? ProfilePictureUrl { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public bool IsPhoneVerified { get; private set; }

        public Rating? OverallRating { get; private set; }
        public Address? Address { get; private set; }

        private readonly List<string> _roles = new();
        public IReadOnlyCollection<string> Roles => _roles.AsReadOnly();

        private readonly List<IdentityDocument> _identityDocuments = new();
        public IReadOnlyCollection<IdentityDocument> IdentityDocuments => _identityDocuments.AsReadOnly();

        private User() { } // EF Core

        public User(string email, string firstName, string lastName, string passwordHash)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            PasswordHash = passwordHash;
        }

        public void UpdateProfile(string firstName, string lastName, string? phone, string? bio, string? profilePictureUrl, Address? address)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Bio = bio;
            ProfilePictureUrl = profilePictureUrl;
            Address = address;
        }

        public void AddRole(string role)
        {
            if (!_roles.Contains(role))
                _roles.Add(role);
        }

        public void VerifyEmail()
        {
            IsEmailVerified = true;
        }

        public void VerifyPhone()
        {
            IsPhoneVerified = true;
        }

        public void AddIdentityDocument(IdentityDocument document)
        {
            _identityDocuments.Add(document);
        }
    }
}
