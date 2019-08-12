using MedPortal.Data.Business.SearchParameters;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MedPortal.Data.Repositories
{
    public class DoctorRepository : Repository<HDoctor>, IDoctorRepository
    {
        public DoctorRepository(IDataContext dataContext) : base(dataContext)
        {
            
        }

        public async Task<List<HDoctor>> FilterDoctorsAsync(LocationSearchParameters locationSearchParameters, string speciality)
        {
            var query = _dbSet
               .Include(c => c.City)
               .Include(c => c.Clinics)
               .ThenInclude(c => c.Clinic)
               .ThenInclude(c => c.HCity)
               .Include(c => c.Specialities)
               .ThenInclude(c => c.Speciality)
               .AsQueryable();
            if (!string.IsNullOrEmpty(locationSearchParameters.City))
            {
                query = query.Where(c => c.City.Alias == locationSearchParameters.City);
            }
            if (!string.IsNullOrEmpty(speciality))
            {
                query = query.Where(c => c.Specialities.Any(s => s.Speciality.Alias == speciality));
            }
            if (locationSearchParameters.LocationType.HasValue && !string.IsNullOrEmpty(locationSearchParameters.Location))
            {
                switch (locationSearchParameters.LocationType.Value)
                {
                    case LocationTypeEnum.District:
                        query = query.Where(d => d.Clinics.Select(c => c.Clinic).Any(c => c.HDistrict.Alias == locationSearchParameters.Location));
                        break;
                    case LocationTypeEnum.Street:
                        query = query.Where(d => d.Clinics.Select(c => c.Clinic).Any(c => c.HStreet.Alias == locationSearchParameters.Location));
                        break;
                    case LocationTypeEnum.MetroStation:
                        query = query.Where(d => d.Clinics.Select(c => c.Clinic).Any(c => c.Stations.Any(s => s.Station.Alias == locationSearchParameters.Location)));
                        break;
                }

            }
            if (locationSearchParameters.InRange.HasValue && locationSearchParameters.Latitude.HasValue && locationSearchParameters.Longitude.HasValue)
            {
                const double oneKilometer = 0.015060;
                query = query.Where(d => d.Clinics.Select(c => c.Clinic).Any(
                    c => c.Latitude >= locationSearchParameters.Latitude.Value - oneKilometer * locationSearchParameters.InRange.Value
                    && c.Latitude <= locationSearchParameters.Latitude.Value + oneKilometer * locationSearchParameters.InRange.Value
                    && c.Longitude >= locationSearchParameters.Longitude.Value - oneKilometer * locationSearchParameters.InRange.Value
                    && c.Longitude <= locationSearchParameters.Longitude.Value + oneKilometer * locationSearchParameters.InRange.Value
                    ));
            }
            return await query.ToListAsync();
        }

        public override Task<List<HDoctor>> GetAsync(Expression<Func<HDoctor, bool>> predicate = null)
        {
            var query = _dbSet.AsNoTracking().Include(c => c.City);
            return predicate != null ? query.Where(predicate).ToListAsync() : query.ToListAsync();
        }
    }
}
