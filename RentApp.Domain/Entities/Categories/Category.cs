using System;
using System.Collections.Generic;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Categories
{
    public class Category : SoftDeleteEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Guid? ParentId { get; private set; }

        private readonly List<Category> _subcategories = new();
        public IReadOnlyCollection<Category> Subcategories => _subcategories.AsReadOnly();

        private Category() { } // EF Core

        public Category(string name, string description, Guid? parentId = null)
        {
            Name = name;
            Description = description;
            ParentId = parentId;
        }

        public void Update(string name, string description, Guid? parentId)
        {
            Name = name;
            Description = description;
            ParentId = parentId;
        }

        public void AddSubcategory(Category category)
        {
            _subcategories.Add(category);
        }
    }
}
