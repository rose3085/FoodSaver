using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Data
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteById<TPrimaryKey>(TPrimaryKey id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetById<TPrimaryKey>(TPrimaryKey id);
        Task<IEnumerable<T>> GetRandomWithIncludeAsync(Expression<Func<T, object>>[] children);
        Task<T> GetByName(Expression<Func<T, bool>> filter);
        Task<bool> EmailExists(Expression<Func<T, bool>> filter);
        Task<IEnumerable<T>> GetWithInclude(Expression<Func<T, object>>[] children);
        //Task<T> GetWithIncludeAndFilter(
        //    Expression<Func<T, object>>[] children,
        //    Expression<Func<T, string>> filter
        //);

        Task<T> GetWithIncludeAndId<TPrimaryKey>(TPrimaryKey id, Expression<Func<T, object>>[] children);

        Task<IEnumerable<T>> GetwithIncludeAndFilter(Expression<Func<T, object>>[] children, Expression<Func<T, bool>> filter);
    }
}
