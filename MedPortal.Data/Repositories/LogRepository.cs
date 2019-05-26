using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;

namespace MedPortal.Data.Repositories {
	public class LogRepository : IRepository<Log> {
		private readonly IDataContext _dataContext;

		public LogRepository(IDataContext dataContext) {
			_dataContext = dataContext;
		}

		public Task<List<Log>> GetAsync(Expression<Func<Log, bool>> predicate = null) {
			throw new NotImplementedException();
		}

		public Task<Log> FindAsync(Expression<Func<Log, bool>> predicate) {
			throw new NotImplementedException();
		}

		public Task<Log> FindByOriginIdAsync(long originId) {
			throw new NotImplementedException();
		}

		public async Task AddAsync(Log item) {
			await _dataContext.Logs.AddAsync(item);
		}

		public void Delete(Log item) {
			throw new NotImplementedException();
		}
	}
}