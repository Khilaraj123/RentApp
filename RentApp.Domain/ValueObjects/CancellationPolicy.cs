using RentApp.Domain.Common;
using System;
using System.Collections.Generic;

namespace RentApp.Domain.ValueObjects
{
    public class CancellationPolicy : ValueObject
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        private CancellationPolicy() { }

        public CancellationPolicy(string name, string description)
        {
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name cannot be empty.", nameof(name)) : name;
            Description = string.IsNullOrWhiteSpace(description) ? throw new ArgumentException("Description cannot be empty.", nameof(description)) : description;
        }

        public static CancellationPolicy Flexible => new CancellationPolicy("Flexible", "Full refund 1 day prior to arrival.");
        public static CancellationPolicy Moderate => new CancellationPolicy("Moderate", "Full refund 5 days prior to arrival.");
        public static CancellationPolicy Strict => new CancellationPolicy("Strict", "Full refund for cancellations made within 48 hours of booking, if the check-in date is at least 14 days away.");

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Name;
            yield return Description;
        }
    }
}
