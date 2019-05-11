using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedPortal.Data.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        Task<IList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);
        
        Task<T> FindByOriginIdAsync(long originId);

        Task AddAsync(T item);

        void Delete(T item);
    }
}