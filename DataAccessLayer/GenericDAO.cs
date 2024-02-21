using DataAccessLayer.Interface;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccessLayer
{
    public class GenericDAO<T> : IGenericDAO<T> where T : class
    {
        private readonly FuminiHotelManagementContext _fuminiHotelManagementContext;
        private readonly DbSet<T> _dbSet;

        public GenericDAO()
        {
            _fuminiHotelManagementContext = new FuminiHotelManagementContext();
            _dbSet = _fuminiHotelManagementContext.Set<T>();
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<T> GetByProperty(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _fuminiHotelManagementContext.SaveChangesAsync();
            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _fuminiHotelManagementContext.SaveChangesAsync();
            return entity;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await _fuminiHotelManagementContext.SaveChangesAsync();
        }

        public async Task<ICollection<T>> GetByProperties(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> condition)
        {
            return _dbSet.Where(condition).AsQueryable().AsNoTracking();
        }

        public async Task<ICollection<T>> GetListByProperty(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<IList<T>> GetAllAsyncInclude(Expression<Func<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (include != null)
            {
                query = query.Include(include);
            }

            return await query.ToListAsync();
        }
    }
}