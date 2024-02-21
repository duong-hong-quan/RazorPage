using System.Linq.Expressions;

namespace DataAccessLayer.Interface
{
    public interface IGenericDAO<T>
    {
        Task<T> GetByIdAsync(int id);

        IQueryable<T> GetByCondition(Expression<Func<T, bool>> condition);

        Task<ICollection<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task<T> UpdateAsync(T entity);

        Task<int> DeleteAsync(T entity);

        Task<T> GetByProperty(Expression<Func<T, bool>> predicate);

        Task<ICollection<T>> GetListByProperty(Expression<Func<T, bool>> predicate);

        Task<IList<T>> GetAllAsyncInclude(Expression<Func<T, object>> include = null);
    }
}