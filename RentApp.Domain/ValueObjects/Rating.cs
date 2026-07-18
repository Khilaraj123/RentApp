using RentApp.Domain.Common;
using System;
using System.Collections.Generic;

namespace RentApp.Domain.ValueObjects
{
    public class Rating : ValueObject
    {
        public double Value { get; private set; }

        private Rating() { }

        public Rating(double value)
        {
            if (value < 1.0 || value > 5.0)
                throw new ArgumentException("Rating value must be between 1.0 and 5.0.", nameof(value));

            Value = Math.Round(value, 1);
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
