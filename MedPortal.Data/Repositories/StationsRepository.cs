using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using MedPortal.Data.Repositories.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MedPortal.Data.Repositories {
	public class StationsRepository : Repository<HStation> {
		public StationsRepository(IDataContext dataContext) : base(dataContext) {
		}

		protected override async Task BulkUpdateInternalAsync(IList<HStation> items) {
			var cities = _dataContext.Cities.ToList();
			using (var transaction = _dataContext.BeginTransaction())
			{
				var existedItems = _dbSet.Include(c => c.City).ToList();
				
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
						existed.City = cities.First(c => c.Id == item.CityId);
					}
				}

				await _dataContext.SaveChangesAsync();
				transaction.Commit();
			}
		}
	}
}