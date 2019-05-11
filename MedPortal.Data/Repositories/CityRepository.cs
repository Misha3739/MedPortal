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
    public class CityRepository : IHighloadedRepository<HCity>
    {
        private readonly IDataContext _dataContext;

        public CityRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task<IList<HCity>> GetAsync(Expression<Func<HCity, bool>> predicate)
        {
            var result = await _dataContext.Cities.AsNoTracking().WhereAsync(predicate);
            return await result.ToListAsync();
        }

        public async Task<HCity> FindAsync(Expression<Func<HCity, bool>> predicate)
        {
            return await _dataContext.Cities.FirstOrDefaultAsync(predicate);
        }

        public async Task<HCity> FindByOriginIdAsync(long originId)
        {
            return await _dataContext.Cities.FirstOrDefaultAsync(c => c.OriginId == originId);
        }

        public async Task AddAsync(HCity item)
        {
            if (item.Id == 0)
            {
               await _dataContext.Cities.AddAsync(item);
            }
        }

        public void Delete(HCity item)
        {
            _dataContext.Cities.Remove(item);
        }

        public async Task BulkUpdate(IList<HCity> items)
        {
            using (var transaction = _dataContext.BeginTransaction())
            {
                foreach (var city in items)
                {
                    if (!(city.OriginId.HasValue && await FindByOriginIdAsync(city.OriginId.Value) != null))
                    {
                        await _dataContext.Cities.AddAsync(city);
                    }
                }

                await _dataContext.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}