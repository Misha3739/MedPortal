using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using MedPortal.Data.Repositories.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MedPortal.Data.Repositories {
	public class HighloadedRepository<T> : Repository<T>, IHighloadedRepository<T> where T : class, IHEntity {
		public HighloadedRepository(IDataContext dataContext) : base(dataContext) {
		}

		public async Task<T> FindByOriginIdAsync(long originId) {
			return await _dbSet.FirstOrDefaultAsync(c => c.OriginId == originId);
		}

		public async Task BulkUpdateAsync(IList<T> items) {
			await BulkUpdateInternalAsync(items);
		}

		public async Task CheckConstraints(bool value) {
			string tableName = _dataContext.GetTableName(_dbSet);
			if (value) {
				await _dataContext.ExecuteSqlCommand($"ALTER TABLE {tableName} CHECK CONSTRAINT ALL");
			} else {
				await _dataContext.ExecuteSqlCommand($"ALTER TABLE {tableName} NOCHECK CONSTRAINT ALL");
			}
		}

		protected virtual async Task BulkUpdateInternalAsync(IList<T> items) {
			if (items == null || !items.Any())
				return;
			;
			using (var transaction = _dataContext.BeginTransaction()) {
				var existedItems = _dbSet.ToList();
				foreach (var item in items) {
					var existed = existedItems.FirstOrDefault(i => i.OriginId == item.OriginId);
					if (existed == null) {
						await _dbSet.AddAsync(item);
					} else {
						ReflectionUtils.Copy(item, existed);
					}
				}

				await _dataContext.SaveChangesAsync();
				transaction.Commit();
			}
		}
	}
}