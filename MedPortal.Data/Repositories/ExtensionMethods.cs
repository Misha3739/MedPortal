using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedPortal.Data.Repositories
{
    public static class ExtensionMethods
    {
        public static async Task<IQueryable<T>> WhereAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> selector)
        {
            return await Task.Run(() => source.Where(selector));
        }
    }
}