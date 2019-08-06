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
    public class ClinicRepository : Repository<HClinic>
    {
        public ClinicRepository(IDataContext dataContext) : base(dataContext)
        {
        }

        public override Task<List<HClinic>> GetAsync(Expression<Func<HClinic, bool>> predicate = null)
        {
            var query = _dbSet.AsNoTracking().Include(c => c.HCity);
            return predicate != null ? query.Where(predicate).ToListAsync() : query.ToListAsync();
        }
    }
}
