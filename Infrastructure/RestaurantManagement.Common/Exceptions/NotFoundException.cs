// File: RestaurantManagement.Common/Exceptions/NotFoundException.cs
namespace RestaurantManagement.Common.Exceptions;

/// <summary>
/// Exception thrown when an entity cannot be found.
/// </summary>
public class NotFoundException : DomainException
{
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" ({key}) was not found.") { }
}
