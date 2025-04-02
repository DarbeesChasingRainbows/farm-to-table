// File: RestaurantManagement.Common/Exceptions/BusinessRuleViolationException.cs
namespace RestaurantManagement.Common.Exceptions;

/// <summary>
/// Exception thrown when a business rule is violated.
/// </summary>
public class BusinessRuleViolationException : DomainException
{
    public BusinessRuleViolationException(string message)
        : base(message) { }
}
