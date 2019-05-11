using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedPortal.Data.Repositories
{
    public class Repository<T> : IHighloadedRepository<T> where T : class, IHEntity
    {
        private readonly IDataContext _dataContext;

        private readonly DbSet<T> _dbSet;

        public Repository(IDataContext dataContext)
        {
            _dataContext = dataContext;
            _dbSet = _dataContext.Set<T>();
        }


        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate = null)
        {
            var items = _dbSet.AsNoTracking();
            var result = predicate != null ? await items.WhereAsync(predicate) : items;
            return await result.ToListAsync();
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<T> FindByOriginIdAsync(long originId)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.OriginId == originId);
        }

        public async Task AddAsync(T item)
        {
            if (item.Id == 0)
            {
               await _dbSet.AddAsync(item);
            }
        }

        public void Delete(T item)
        {
            _dbSet.Remove(item);
        }

        public async Task BulkUpdate(IList<T> items)
        {
            using (var transaction = _dataContext.BeginTransaction())
            {
                foreach (var item in items)
                {
                    if (!(item.OriginId.HasValue && await FindByOriginIdAsync(item.OriginId.Value) != null))
                    {
                        await _dbSet.AddAsync(item);
                    }
                }

                await _dataContext.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}