using System;
using System.Collections.Generic;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Wishlists
{
    public class Wishlist : AuditableEntity
    {
        public Guid UserId { get; private set; }
        private readonly List<WishlistItem> _items = new();
        public IReadOnlyCollection<WishlistItem> Items => _items.AsReadOnly();

        private Wishlist() { } // EF Core

        public Wishlist(Guid userId)
        {
            UserId = userId;
        }

        public void AddItem(WishlistItem item)
        {
            _items.Add(item);
        }
    }
}
