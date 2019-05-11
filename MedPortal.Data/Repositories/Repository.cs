using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using MedPortal.Data.Repositories.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MedPortal.Data.Repositories
{
    public class Repository<T> : IHighloadedRepository<T> where T : class, IHEntity
    {
        protected readonly IDataContext _dataContext;

        protected readonly DbSet<T> _dbSet;

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

        public async Task BulkUpdateAsync(IList<T> items) {
	        await BulkUpdateInternalAsync(items);
        }

        protected virtual async Task BulkUpdateInternalAsync(IList<T> items) {
	        using (var transaction = _dataContext.BeginTransaction())
	        {
		        var existedItems = _dbSet.ToList();
		        foreach (var item in items)
		        {
			        var existed = existedItems.FirstOrDefault(i => i.OriginId == item.OriginId);
			        if (existed == null)
			        {
				        await _dbSet.AddAsync(item);
			        }
			        else
			        {
				        ReflectionUtils.Copy(item, existed);
			        }
		        }

		        await _dataContext.SaveChangesAsync();
		        transaction.Commit();
	        }
		}
    }
}