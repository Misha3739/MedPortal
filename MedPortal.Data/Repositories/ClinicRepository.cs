using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using Microsoft.EntityFrameworkCore;
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
            var result = from clinic in _dbSet
                         join clinicSpecs in _dataContext.ClinicSpecialities.DefaultIfEmpty() on clinic.Id equals clinicSpecs.ClinicId
                         join hspec in _dataContext.Specialities.DefaultIfEmpty() on clinicSpecs.SpecialityId equals hspec.Id
                         join hcity in _dataContext.Cities on clinic.HCityId equals hcity.Id
                         select new { Clinic = clinic, City = hcity, Speciality = hspec };
            if(!string.IsNullOrEmpty(city))
            {
                result = result.Where(c => c.City.Alias == city);
            }
            if (!string.IsNullOrEmpty(speciality))
            {
                result = result.Where(c => c.Speciality != null && c.Speciality.BranchAlias == speciality);
            }
            return await result.Select(c => c.Clinic).Distinct().Include(c => c.HCity).ToListAsync();
        }

        public override Task<List<HClinic>> GetAsync(Expression<Func<HClinic, bool>> predicate = null)
        {
            var query = _dbSet.AsNoTracking().Include(c => c.HCity);
            return predicate != null ? query.Where(predicate).ToListAsync() : query.ToListAsync();
        }
    }
}
