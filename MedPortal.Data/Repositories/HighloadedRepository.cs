using System;
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
			await BulkUpdateInternalAsync(items, false);
		}

		public async Task CheckConstraints(bool value) {
			string tableName = _dataContext.GetTableName(_dbSet);
			if (value) {
				await _dataContext.ExecuteSqlCommand($"ALTER TABLE {tableName} CHECK CONSTRAINT ALL");
			} else {
				await _dataContext.ExecuteSqlCommand($"ALTER TABLE {tableName} NOCHECK CONSTRAINT ALL");
			}
		}

		protected virtual async Task BulkUpdateInternalAsync(IList<T> items, bool assignIds) {
			if (items == null || !items.Any())
				return;

			using (var transaction = _dataContext.BeginTransaction()) {
				var existingOriginIds = _dbSet.Select(c => c.OriginId).ToList();
				var itemsToUpdate = items.Where(item => existingOriginIds.Contains(item.OriginId)).ToList();
				var itemsToInsert = items.Except(itemsToUpdate).ToList();
				try {
					if (itemsToInsert.Any()) {
						await _dataContext.BulkInsertAsync(itemsToInsert);
					}

					await _dataContext.BulkUpdateAsync(itemsToUpdate);
					transaction.Commit();
				} catch (Exception) {
					transaction.Rollback();
				}
				
			}

			if (assignIds) {
				var originIds = items.Select(c => c.OriginId).ToList();
				Dictionary<long, long> ids = _dbSet.Where(c => originIds.Contains(c.OriginId))
					.Select(c => new { c.OriginId, c.Id }).ToDictionary(x => x.OriginId, x => x.Id);
				foreach (var item in items) {
					item.Id = ids[item.OriginId];
				}

			}
		}
	}
}