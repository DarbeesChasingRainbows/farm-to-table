using System.Linq.Expressions;

namespace RestaurantManagement.InventoryService.Infrastructure.Data.Repositories
{
    public interface IRepository<T>
        where T : class
    {
        // Get by ID
        Task<T?> GetByIdAsync(string id);

        // Get all entities
        Task<IReadOnlyList<T>> GetAllAsync();

        // Find entities based on predicate
        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Add a new entity
        Task<T> AddAsync(T entity);

        // Add multiple entities
        Task AddRangeAsync(IEnumerable<T> entities);

        // Update an existing entity
        void Update(T entity);

        // Remove an entity
        void Remove(T entity);

        // Remove multiple entities
        void RemoveRange(IEnumerable<T> entities);

        // Count entities
        Task<int> CountAsync();

        // Count entities based on predicate
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        // Check if any entity exists based on predicate
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}
