using RentApp.Domain.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentApp.Domain.ValueObjects
{
    [ComplexType]
    public class Address : ValueObject
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }
        public string Country { get; private set; }

        private Address() { }

        public Address(string street, string city, string state, string zipCode, string country)
        {
            Street = string.IsNullOrWhiteSpace(street) ? throw new ArgumentException("Street cannot be empty.", nameof(street)) : street;
            City = string.IsNullOrWhiteSpace(city) ? throw new ArgumentException("City cannot be empty.", nameof(city)) : city;
            State = string.IsNullOrWhiteSpace(state) ? throw new ArgumentException("State cannot be empty.", nameof(state)) : state;
            ZipCode = string.IsNullOrWhiteSpace(zipCode) ? throw new ArgumentException("Zip code cannot be empty.", nameof(zipCode)) : zipCode;
            Country = string.IsNullOrWhiteSpace(country) ? throw new ArgumentException("Country cannot be empty.", nameof(country)) : country;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
            yield return State;
            yield return ZipCode;
            yield return Country;
        }
    }
}
