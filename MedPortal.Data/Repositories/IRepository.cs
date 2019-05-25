using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedPortal.Data.DTO;

namespace MedPortal.Data.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate = null);
        
        Task<T> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T item);

        void Delete(T item);
    }
}