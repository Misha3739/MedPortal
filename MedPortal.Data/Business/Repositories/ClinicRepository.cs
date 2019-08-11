using MedPortal.Data.Business.SearchParameters;
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

        public async Task<List<HClinic>> FilterClinicsAsync(LocationSearchParameters locationSearchParameters, string speciality)
        {
            var query = _dbSet
                .Include(c => c.HCity)
                .Include(c => c.HStreet)
                .Include(c => c.Stations)
                .ThenInclude(c => c.Station)
                .Include(c => c.Specialities)
                .ThenInclude(c => c.Speciality)
                .AsQueryable();
            if (!string.IsNullOrEmpty(locationSearchParameters.City))
            {
                query = query.Where(c => c.HCity.Alias == locationSearchParameters.City);
            }
            if (!string.IsNullOrEmpty(speciality))
            {
                query = query.Where(c=> c.Specialities.Any(s => s.Speciality.BranchAlias == speciality));
            }
            if (locationSearchParameters.LocationType.HasValue && !string.IsNullOrEmpty(locationSearchParameters.Location))
            {
                switch(locationSearchParameters.LocationType.Value)
                {
                    case LocationTypeEnum.District:
                        query = query.Where(c => c.HDistrict.Alias == locationSearchParameters.Location);
                        break;
                    case LocationTypeEnum.Street:
                        query = query.Where(c => c.HStreet.Alias == locationSearchParameters.Location);
                        break;
                    case LocationTypeEnum.MetroStation:
                        query = query.Where(c => c.Stations.Any(s => s.Station.Alias  == locationSearchParameters.Location));
                        break;
                }
                
            }
            if (locationSearchParameters.InRange.HasValue && locationSearchParameters.Latitude.HasValue && locationSearchParameters.Longitude.HasValue) {
                const double oneKilometer = 0.015060;
                query = query.Where(c => 
                    c.Latitude >= locationSearchParameters.Latitude.Value - oneKilometer * locationSearchParameters.InRange.Value
                    && c.Latitude <= locationSearchParameters.Latitude.Value + oneKilometer * locationSearchParameters.InRange.Value
                    && c.Longitude >= locationSearchParameters.Longitude.Value - oneKilometer * locationSearchParameters.InRange.Value
                    && c.Longitude <= locationSearchParameters.Longitude.Value + oneKilometer * locationSearchParameters.InRange.Value);
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
