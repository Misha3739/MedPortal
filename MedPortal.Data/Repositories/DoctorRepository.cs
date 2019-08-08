using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<HDoctor>> FilterDoctorsAsync(string city, string speciality)
        {
            var result = from doctor in _dbSet
                         join doctorSpecs in _dataContext.DoctorSpecialities on doctor.Id equals doctorSpecs.DoctorId
                         join hspec in _dataContext.Specialities on doctorSpecs.SpecialityId equals hspec.Id
                         join hcity in _dataContext.Cities on doctor.CityId equals hcity.Id
                         select new { Doctor = doctor, City = hcity, Speciality = hspec };

            if (!string.IsNullOrEmpty(city))
            {
                result = result.Where(c => c.City.Alias == city);
            }
            if (!string.IsNullOrEmpty(speciality))
            {
                result = result.Where(c => c.Speciality != null && c.Speciality.Alias == speciality);
            }
            return await result.Select(c => c.Doctor).Distinct().Include(c => c.City).Include(c => c.Specialities).ToListAsync();
        }

        public override Task<List<HDoctor>> GetAsync(Expression<Func<HDoctor, bool>> predicate = null)
        {
            var query = _dbSet.AsNoTracking().Include(c => c.City);
            return predicate != null ? query.Where(predicate).ToListAsync() : query.ToListAsync();
        }
    }
}
