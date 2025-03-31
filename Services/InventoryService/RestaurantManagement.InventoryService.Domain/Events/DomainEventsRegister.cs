using System;
using System.Collections.Generic;
using System.Reflection;

namespace RestaurantManagement.InventoryService.Domain.Events
{
    /// <summary>
    /// Provides a registry of all domain events in the system.
    /// </summary>
    public static class DomainEventsRegister
    {
        private static readonly Dictionary<string, Type> _eventTypes =
            new Dictionary<string, Type>();

        /// <summary>
        /// Initializes the DomainEventsRegister.
        /// </summary>
        static DomainEventsRegister()
        {
            RegisterAllEventTypes();
        }

        /// <summary>
        /// Gets the type of an event by its name.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>The type of the event, or null if not found.</returns>
        public static Type GetEventType(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            return _eventTypes.TryGetValue(eventName, out var eventType) ? eventType : null;
        }

        /// <summary>
        /// Gets all registered event types.
        /// </summary>
        /// <returns>A collection of all registered event types.</returns>
        public static IReadOnlyCollection<Type> GetAllEventTypes() =>
            new List<Type>(_eventTypes.Values).AsReadOnly();

        private static void RegisterAllEventTypes()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var eventTypes = assembly.GetTypes();

            foreach (var type in eventTypes)
            {
                if (typeof(IDomainEvent).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                {
                    _eventTypes.Add(type.Name, type);
                }
            }
        }
    }
}
