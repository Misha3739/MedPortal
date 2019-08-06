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
                         join clinicDoctors in _dataContext.ClinicDoctors on clinic.Id equals clinicDoctors.ClinicId
                         join doctorsSpecialitis in _dataContext.DoctorSpecialities on clinicDoctors.DoctorId equals doctorsSpecialitis.DoctorId
                         join hspec in _dataContext.Specialities on doctorsSpecialitis.SpecialityId equals hspec.Id
                         join hcity in _dataContext.Cities on clinic.HCityId equals hcity.Id
                         where hspec.Alias == speciality && hcity.Alias == city select clinic;
            return await result.Include(c => c.HCity).ToListAsync();
        }

        public override Task<List<HClinic>> GetAsync(Expression<Func<HClinic, bool>> predicate = null)
        {
            var query = _dbSet.AsNoTracking().Include(c => c.HCity);
            return predicate != null ? query.Where(predicate).ToListAsync() : query.ToListAsync();
        }
    }
}
