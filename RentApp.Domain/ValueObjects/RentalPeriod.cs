using RentApp.Domain.Common;
using System;
using System.Collections.Generic;

namespace RentApp.Domain.ValueObjects
{
    [System.ComponentModel.DataAnnotations.Schema.ComplexType]
    public class RentalPeriod : ValueObject
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        private RentalPeriod() { }

        public RentalPeriod(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
                throw new ArgumentException("Start date must be before end date.");

            StartDate = startDate;
            EndDate = endDate;
        }

        public int TotalDays => (EndDate - StartDate).Days;

        public bool OverlapsWith(RentalPeriod other)
        {
            return StartDate < other.EndDate && EndDate > other.StartDate;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return StartDate;
            yield return EndDate;
        }
    }
}
