using FluentValidation.Results;

namespace RestaurantManagement.InventoryService.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when validation fails
    /// </summary>
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            var failureGroups = failures.GroupBy(e => e.PropertyName, e => e.ErrorMessage);

            foreach (var failureGroup in failureGroups)
            {
                var propertyName = failureGroup.Key;
                var propertyFailures = failureGroup.ToArray();

                Errors.Add(propertyName, propertyFailures);
            }
        }
    }

    /// <summary>
    /// Exception thrown when an entity is not found
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException()
            : base() { }

        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException) { }

        public NotFoundException(string name, object key)
            : base($"Entity '{name}' ({key}) was not found.") { }
    }

    /// <summary>
    /// Exception thrown when access is forbidden
    /// </summary>
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException()
            : base() { }
    }

    /// <summary>
    /// Exception thrown when a business rule is violated
    /// </summary>
    public class BusinessRuleViolationException : Exception
    {
        public BusinessRuleViolationException(string message)
            : base(message) { }
    }
}
