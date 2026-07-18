using RentApp.Domain.Common;
using System;
using System.Collections.Generic;

namespace RentApp.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        private Money() { }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty.", nameof(currency));

            Amount = amount;
            Currency = currency;
        }

        public static Money Zero(string currency = "USD") => new Money(0, currency);

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot add money with different currencies.");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new InvalidOperationException("Cannot subtract money with different currencies.");
            if (Amount < other.Amount)
                throw new InvalidOperationException("Cannot subtract more than the current amount.");

            return new Money(Amount - other.Amount, Currency);
        }
    }
}
