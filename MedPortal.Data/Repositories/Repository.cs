using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using MedPortal.Data.Repositories.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MedPortal.Data.Repositories {
	public class Repository<T> : IRepository<T> where T : class, IEntity {
		protected readonly IDataContext _dataContext;

		protected readonly DbSet<T> _dbSet;

		public Repository(IDataContext dataContext) {
			_dataContext = dataContext;
			_dbSet = _dataContext.Set<T>();
		}

		public virtual async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate = null) {
			var items = _dbSet.AsNoTracking();
			var result = predicate != null ? await items.WhereAsync(predicate) : items;
			return await result.ToListAsync();
		}

		public virtual async Task<T> FindAsync(Expression<Func<T, bool>> predicate) {
			return await _dbSet.FirstOrDefaultAsync(predicate);
		}

		public async Task AddAsync(T item) {
			if (item.Id == 0) {
				await _dbSet.AddAsync(item);
			}
		}

		public void Delete(T item) {
			_dbSet.Remove(item);
		}
	}
}