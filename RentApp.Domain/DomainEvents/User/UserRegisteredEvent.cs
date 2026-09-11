using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.User
{
    public class UserRegisteredEvent : DomainEvent
    {
        public Guid UserId { get; }
        public string FullName { get; }
        public string? Email { get; }

        public UserRegisteredEvent(Guid userId, string fullName, string? email)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
        }
    }
}
