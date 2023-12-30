using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace HospitalManagement.Data
{
    public interface IEntityBase
    {
        int Id { get; set; }
    }

    public interface IEntityBaseRepo<T> where T : class, IEntityBase, new()
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperities);
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
    }

    public class EntityBaseRepository<T> : IEntityBaseRepo<T> where T : class, IEntityBase, new()
    {
        private readonly ApplicationDbContext _context;

        public EntityBaseRepository(ApplicationDbContext context) => _context = context;

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            EntityEntry entityEntry = _context.Entry<T>(await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id));
            entityEntry.State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperities)
        {
            IQueryable<T> query = _context.Set<T>();
            query = includeProperities.Aggregate(query, (current, includeProperities) => current.Include(includeProperities));
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id) => await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

        public async Task UpdateAsync(int id, T entity)
        {
            EntityEntry entityEntry = _context.Entry<T>(entity);
            entityEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
