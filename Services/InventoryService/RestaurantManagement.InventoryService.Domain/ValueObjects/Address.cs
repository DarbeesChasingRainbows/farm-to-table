using System;
using RestaurantManagement.InventoryService.Domain.Exceptions;

namespace RestaurantManagement.InventoryService.Domain.ValueObjects
{
    public class Address
    {
        private Address() { }

        public Address(string street, string city, string state, string postalCode, string country)
        {
            Street = street;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }

        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }

        public static Address Empty => new Address(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Address other = (Address)obj;
            return Street == other.Street &&
                   City == other.City &&
                   State == other.State &&
                   PostalCode == other.PostalCode &&
                   Country == other.Country;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Street, City, State, PostalCode, Country);
        }

        public override string ToString()
        {
            return $"{Street}, {City}, {State} {PostalCode}, {Country}";
        }

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Street) &&
                   string.IsNullOrWhiteSpace(City) &&
                   string.IsNullOrWhiteSpace(State) &&
                   string.IsNullOrWhiteSpace(PostalCode) &&
                   string.IsNullOrWhiteSpace(Country);
        }

        public string GetFormattedAddress(bool includeCountry = true)
        {
            string formatted = string.Empty;

            if (!string.IsNullOrWhiteSpace(Street))
                formatted += Street;

            if (!string.IsNullOrWhiteSpace(City))
            {
                if (!string.IsNullOrWhiteSpace(formatted))
                    formatted += ", ";
                formatted += City;
            }

            if (!string.IsNullOrWhiteSpace(State))
            {
                if (!string.IsNullOrWhiteSpace(formatted))
                    formatted += ", ";
                formatted += State;
            }

            if (!string.IsNullOrWhiteSpace(PostalCode))
            {
                if (!string.IsNullOrWhiteSpace(State))
                    formatted += " ";
                else if (!string.IsNullOrWhiteSpace(formatted))
                    formatted += ", ";
                formatted += PostalCode;
            }

            if (includeCountry && !string.IsNullOrWhiteSpace(Country))
            {
                if (!string.IsNullOrWhiteSpace(formatted))
                    formatted += ", ";
                formatted += Country;
            }

            return formatted;
        }
    }
}
