using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MedPortal.Data.Persistence {
	public class UnitOfWork : IUnitOfWork {
		private readonly IDataContext _dataContext;

		public UnitOfWork(IDataContext dataContext) {
			_dataContext = dataContext;
		}

		public async Task<int> SaveChangesAsync() {
			return await _dataContext.SaveChangesAsync();
		}

		public async Task CheckConstraints<T>(bool value) {
			var model = _dataContext.Model;
			var entityTypes = model.GetEntityTypes();
			var entityType = entityTypes.First(t => t.ClrType == typeof(T));
			var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
			var tableName = tableNameAnnotation.Value.ToString();

			if (value) {
				await _dataContext.ExecuteSqlCommand($"ALTER TABLE {tableName} CHECK CONSTRAINT ALL");
			} else {
				await _dataContext.ExecuteSqlCommand($"ALTER TABLE {tableName} NOCHECK CONSTRAINT ALL");
			}
		}
	}
}