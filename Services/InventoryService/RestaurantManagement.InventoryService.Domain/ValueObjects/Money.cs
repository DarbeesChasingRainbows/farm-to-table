using System;
using System.Globalization;
using RestaurantManagement.InventoryService.Domain.Exceptions;

namespace RestaurantManagement.InventoryService.Domain.ValueObjects
{
    public class Money
    {
        private Money() { }

        public Money(decimal amount, string currency = "USD")
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new InventoryDomainException("Currency cannot be empty");

            // Round to 2 decimal places
            Amount = Math.Round(amount, 2);
            Currency = currency;
        }

        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public static Money Zero(string currency = "USD") => new Money(0, currency);

        public static Money FromString(string value, string currency = "USD")
        {
            if (string.IsNullOrWhiteSpace(value))
                return Zero(currency);

            if (decimal.TryParse(value, NumberStyles.Currency, CultureInfo.InvariantCulture, out decimal amount))
                return new Money(amount, currency);

            throw new InventoryDomainException($"Cannot convert '{value}' to Money");
        }

        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InventoryDomainException("Cannot add Money with different currencies");

            return new Money(left.Amount + right.Amount, left.Currency);
        }

        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InventoryDomainException("Cannot subtract Money with different currencies");

            return new Money(left.Amount - right.Amount, left.Currency);
        }

        public static Money operator *(Money money, decimal multiplier)
        {
            return new Money(money.Amount * multiplier, money.Currency);
        }

        public static Money operator *(decimal multiplier, Money money)
        {
            return money * multiplier;
        }

        public static Money operator /(Money money, decimal divisor)
        {
            if (divisor == 0)
                throw new DivideByZeroException();

            return new Money(money.Amount / divisor, money.Currency);
        }

        public static bool operator ==(Money left, Money right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            if (ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Money left, Money right)
        {
            return !(left == right);
        }

        public static bool operator >(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InventoryDomainException("Cannot compare Money with different currencies");

            return left.Amount > right.Amount;
        }

        public static bool operator <(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InventoryDomainException("Cannot compare Money with different currencies");

            return left.Amount < right.Amount;
        }

        public static bool operator >=(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InventoryDomainException("Cannot compare Money with different currencies");

            return left.Amount >= right.Amount;
        }

        public static bool operator <=(Money left, Money right)
        {
            if (left.Currency != right.Currency)
                throw new InventoryDomainException("Cannot compare Money with different currencies");

            return left.Amount <= right.Amount;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Money other = (Money)obj;
            return Currency == other.Currency && Amount == other.Amount;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Currency);
        }

        public override string ToString()
        {
            return $"{Amount.ToString("F2", CultureInfo.InvariantCulture)} {Currency}";
        }

        public string ToFormattedString()
        {
            CultureInfo culture = Currency == "USD" ? new CultureInfo("en-US") :
                                 Currency == "EUR" ? new CultureInfo("fr-FR") :
                                 Currency == "GBP" ? new CultureInfo("en-GB") :
                                 CultureInfo.InvariantCulture;

            return Amount.ToString("C", culture);
        }
    }
}
