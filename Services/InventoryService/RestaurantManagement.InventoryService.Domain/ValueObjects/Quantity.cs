using System;
using RestaurantManagement.InventoryService.Domain.Exceptions;

namespace RestaurantManagement.InventoryService.Domain.ValueObjects
{
    public class Quantity
    {
        private Quantity() { }

        public Quantity(double amount, string unit)
        {
            if (amount < 0)
                throw new InventoryDomainException("Quantity amount cannot be negative");

            if (string.IsNullOrWhiteSpace(unit))
                throw new InventoryDomainException("Quantity unit cannot be empty");

            Amount = amount;
            Unit = unit;
        }

        public double Amount { get; private set; }
        public string Unit { get; private set; }

        public static Quantity Zero(string unit) => new Quantity(0, unit);

        public static Quantity operator +(Quantity left, Quantity right)
        {
            if (left.Unit != right.Unit)
                throw new InventoryDomainException("Cannot add Quantity with different units");

            return new Quantity(left.Amount + right.Amount, left.Unit);
        }

        public static Quantity operator -(Quantity left, Quantity right)
        {
            if (left.Unit != right.Unit)
                throw new InventoryDomainException("Cannot subtract Quantity with different units");

            double result = left.Amount - right.Amount;
            if (result < 0)
                result = 0;

            return new Quantity(result, left.Unit);
        }

        public static Quantity operator *(Quantity quantity, double multiplier)
        {
            return new Quantity(quantity.Amount * multiplier, quantity.Unit);
        }

        public static Quantity operator *(double multiplier, Quantity quantity)
        {
            return quantity * multiplier;
        }

        public static Quantity operator /(Quantity quantity, double divisor)
        {
            if (divisor == 0)
                throw new DivideByZeroException();

            return new Quantity(quantity.Amount / divisor, quantity.Unit);
        }

        public static bool operator ==(Quantity left, Quantity right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            if (ReferenceEquals(right, null))
                return false;

            return left.Equals(right);
        }

        public static bool operator !=(Quantity left, Quantity right)
        {
            return !(left == right);
        }

        public static bool operator >(Quantity left, Quantity right)
        {
            if (left.Unit != right.Unit)
                throw new InventoryDomainException("Cannot compare Quantity with different units");

            return left.Amount > right.Amount;
        }

        public static bool operator <(Quantity left, Quantity right)
        {
            if (left.Unit != right.Unit)
                throw new InventoryDomainException("Cannot compare Quantity with different units");

            return left.Amount < right.Amount;
        }

        public static bool operator >=(Quantity left, Quantity right)
        {
            if (left.Unit != right.Unit)
                throw new InventoryDomainException("Cannot compare Quantity with different units");

            return left.Amount >= right.Amount;
        }

        public static bool operator <=(Quantity left, Quantity right)
        {
            if (left.Unit != right.Unit)
                throw new InventoryDomainException("Cannot compare Quantity with different units");

            return left.Amount <= right.Amount;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Quantity other = (Quantity)obj;
            return Unit == other.Unit && Math.Abs(Amount - other.Amount) < 0.00001;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Amount, Unit);
        }

        public override string ToString()
        {
            return $"{Amount:0.##} {Unit}";
        }

        public bool IsZero()
        {
            return Math.Abs(Amount) < 0.00001;
        }

        public Quantity ConvertTo(string targetUnit, double conversionFactor)
        {
            if (Unit == targetUnit)
                return this;

            return new Quantity(Amount * conversionFactor, targetUnit);
        }
    }
}
