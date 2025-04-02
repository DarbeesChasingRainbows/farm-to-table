// File: RestaurantManagement.Common/Exceptions/ForbiddenAccessException.cs
namespace RestaurantManagement.Common.Exceptions;

/// <summary>
/// Exception thrown when access to a resource is forbidden.
/// </summary>
public class ForbiddenAccessException : DomainException
{
    public ForbiddenAccessException()
        : base("You do not have permission to access this resource.") { }
}
