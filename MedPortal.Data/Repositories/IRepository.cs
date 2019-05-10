using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedPortal.Data.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        Task<IList<T>> Get(Expression<Func<T, bool>> predicate);
    }
}