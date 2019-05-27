using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
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
			
			using (var transaction = _dataContext.BeginTransaction()) {
				var existingOriginIds = _dbSet.Select(c => c.OriginId).ToList();
				var itemsToUpdate = items.Where(item => existingOriginIds.Contains(item.OriginId)).ToList();
				var itemsToInsert = items.Except(itemsToUpdate).ToList();
				if (itemsToInsert.Any()) {
					await _dataContext.BulkInsertAsync(itemsToInsert);
				}
				await _dataContext.BulkUpdateAsync(itemsToUpdate);

				transaction.Commit();
			}
		}
	}
}