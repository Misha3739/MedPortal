using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedPortal.Data.Repositories
{
    public class ClinicRepository : Repository<HClinic>, IClinicRepository
    {
        public ClinicRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public async Task<List<HClinic>> FilterClinicsAsync(string city, string speciality)
        {
            var query = _dbSet
                .Include(c => c.HCity)
                .Include(c => c.HStreet)
                .Include(c => c.Stations)
                .ThenInclude(c => c.Station)
                .Include(c => c.Specialities)
                .ThenInclude(c => c.Speciality)
                .AsQueryable();
            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(c => c.HCity.Alias == city);
            }
            if (!string.IsNullOrEmpty(speciality))
            {
                query = query.Where(c=> c.Specialities.Any(s => s.Speciality.BranchAlias == speciality));
            }
            return await query.ToListAsync();
        }

        public override Task<List<HClinic>> GetAsync(Expression<Func<HClinic, bool>> predicate = null)
        {
            var query = _dbSet.AsNoTracking().Include(c => c.HCity);
            return predicate != null ? query.Where(predicate).ToListAsync() : query.ToListAsync();
        }
    }
}
