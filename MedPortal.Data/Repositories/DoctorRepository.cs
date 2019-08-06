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
    public class DoctorRepository : Repository<HDoctor>
    {
        public DoctorRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public override Task<List<HDoctor>> GetAsync(Expression<Func<HDoctor, bool>> predicate = null)
        {
            var query = _dbSet.AsNoTracking().Include(c => c.City);
            return predicate != null ? query.Where(predicate).ToListAsync() : query.ToListAsync();
        }
    }
}
