using Microsoft.EntityFrameworkCore;

namespace API_Template.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while fetching all entities.", ex);
            }
        }

        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"Entity with id {id} not found.");
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error occurred while fetching the entity with id {id}.", ex);
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while adding the entity.", ex);
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error occurred while updating the entity.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    throw new KeyNotFoundException($"Entity with id {id} not found.");
                }

                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error occurred while deleting the entity with id {id}.", ex);
            }
        }
    }

}
