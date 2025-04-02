// File: RestaurantManagement.Common/Exceptions/DomainException.cs
namespace RestaurantManagement.Common.Exceptions;

/// <summary>
/// Base exception for domain-specific errors.
/// </summary>
public abstract class DomainException : Exception
{
    protected DomainException(string message)
        : base(message) { }
}
