using System;
using System.Linq;
using System.Collections.Generic;
using RentApp.Domain.Common;
using RentApp.Domain.DomainEvents;
using RentApp.Domain.Entities.Listings;

namespace RentApp.Domain.Entities.Categories
{
    public class Category : SoftDeleteEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Slug { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        
        public Guid? ParentId { get; private set; }
        public virtual Category? Parent { get; private set; }
        
        public int DisplayOrder { get; private set; }
        public string? IconUrl { get; private set; }
        public string? ImageUrl { get; private set; }
        
        public bool IsActive { get; private set; } = true;
        
        public string? MetaTitle { get; private set; }
        public string? MetaDescription { get; private set; }

        public int ListingCount { get; private set; }

        private readonly List<Category> _subcategories = new();
        public IReadOnlyCollection<Category> Subcategories => _subcategories.AsReadOnly();

        private readonly List<Listing> _listings = new();
        public IReadOnlyCollection<Listing> Listings => _listings.AsReadOnly();

        private Category() { } // EF Core

        public Category(string name, string slug, string description, Guid? parentId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty.", nameof(name));
            if (name.Length > 100)
                throw new ArgumentException("Category name cannot exceed 100 characters.", nameof(name));
                
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Category slug cannot be empty.", nameof(slug));

            Name = name.Trim();
            Slug = slug.Trim().ToLowerInvariant();
            Description = description?.Trim() ?? string.Empty;
            ParentId = parentId;
            IsActive = true;
        }

        public void Rename(string name, string slug, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty.", nameof(name));
            if (name.Length > 100)
                throw new ArgumentException("Category name cannot exceed 100 characters.", nameof(name));
                
            if (string.IsNullOrWhiteSpace(slug))
                throw new ArgumentException("Category slug cannot be empty.", nameof(slug));

            Name = name.Trim();
            Slug = slug.Trim().ToLowerInvariant();
            Description = description?.Trim() ?? string.Empty;

            AddDomainEvent(new CategoryRenamedEvent(Id));
        }

        public void UpdateSEO(string? metaTitle, string? metaDescription)
        {
            MetaTitle = metaTitle?.Trim();
            MetaDescription = metaDescription?.Trim();
        }

        public void UpdateMedia(string? iconUrl, string? imageUrl)
        {
            IconUrl = iconUrl?.Trim();
            ImageUrl = imageUrl?.Trim();
        }

        public void ChangeDisplayOrder(int displayOrder)
        {
            DisplayOrder = displayOrder;
        }

        public void MoveTo(Category? newParent)
        {
            if (newParent != null && newParent.Id == Id)
            {
                throw new InvalidOperationException("A category cannot be its own parent.");
            }

            // Note: Deep descendant validation (preventing A -> B -> C -> A) 
            // is best handled by a Domain Service that queries the database.
            
            ParentId = newParent?.Id;
            Parent = newParent;
        }

        public void Activate()
        {
            if (IsActive) return;
            
            IsActive = true;
            AddDomainEvent(new CategoryActivatedEvent(Id));
        }

        public void Deactivate()
        {
            if (!IsActive) return;
            
            IsActive = false;
            AddDomainEvent(new CategoryDeactivatedEvent(Id));
        }

        public void AddSubcategory(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            if (_subcategories.Any(x => x.Id == category.Id))
                return;

            _subcategories.Add(category);
        }

        public void RemoveSubcategory(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            var existing = _subcategories.FirstOrDefault(x => x.Id == category.Id);
            if (existing != null)
            {
                _subcategories.Remove(existing);
            }
        }
        
        public void UpdateListingCount(int count)
        {
            if (count < 0) throw new ArgumentException("Listing count cannot be negative.", nameof(count));
            ListingCount = count;
        }
    }
}
