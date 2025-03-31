using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RestaurantManagement.InventoryService.Application.Interfaces;

namespace RestaurantManagement.InventoryService.Infrastructure.Services
{
    /// <summary>
    /// Implementation of the current user service.
    /// </summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the current user ID.
        /// </summary>
        public string? UserId =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        /// <summary>
        /// Gets a value indicating whether the current user is authenticated.
        /// </summary>
        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        public IEnumerable<string> Roles
        {
            get
            {
                var userRoles = _httpContextAccessor.HttpContext?.User?.FindAll(ClaimTypes.Role);
                return userRoles?.Select(r => r.Value) ?? Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Checks if the current user has a specific role.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <returns>True if the user has the role, otherwise false.</returns>
        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
        }
    }
}
