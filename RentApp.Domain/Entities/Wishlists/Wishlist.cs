using System;
using System.Collections.Generic;
using System.Linq;
using RentApp.Domain.Common;
using RentApp.Domain.DomainEvents.Wishlist;

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
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.");

            UserId = userId;
        }

        public void AddItem(Guid listingId)
        {
            if (_items.Any(i => i.ListingId == listingId))
                return;

            var item = new WishlistItem(Id, listingId);
            _items.Add(item);

            AddDomainEvent(new WishlistItemAddedEvent(Id, listingId));
        }

        public void RemoveItem(Guid listingId)
        {
            var item = _items.FirstOrDefault(x => x.ListingId == listingId);

            if (item is null)
                return;

            _items.Remove(item);

            AddDomainEvent(new WishlistItemRemovedEvent(Id, listingId));
        }

        public void Clear()
        {
            _items.Clear();

            AddDomainEvent(new WishlistClearedEvent(Id));
        }

        public bool Contains(Guid listingId)
        {
            return _items.Any(i => i.ListingId == listingId);
        }
    }
}
