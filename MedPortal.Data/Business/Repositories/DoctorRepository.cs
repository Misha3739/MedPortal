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

        public async Task<List<HDoctor>> FilterDoctorsAsync(string city, string speciality)
        {
            var query = _dbSet
               .Include(c => c.City)
               .Include(c => c.Clinics)
               .ThenInclude(c => c.Clinic)
               .ThenInclude(c => c.HCity)
               .Include(c => c.Specialities)
               .ThenInclude(c => c.Speciality)
               .AsQueryable();
            if (!string.IsNullOrEmpty(city))
            {
                query = query.Where(c => c.City.Alias == city);
            }
            if (!string.IsNullOrEmpty(speciality))
            {
                query = query.Where(c => c.Specialities.Any(s => s.Speciality.Alias == speciality));
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
