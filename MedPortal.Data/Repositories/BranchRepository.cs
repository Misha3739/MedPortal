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
    public class BranchRepository : IHighloadedRepository<HBranch>
    {
        private readonly IDataContext _dataContext;

        public BranchRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }


        public async Task<IList<HBranch>> GetAsync(Expression<Func<HBranch, bool>> predicate)
        {
            var result = await _dataContext.Branches.AsNoTracking().WhereAsync(predicate);
            return await result.ToListAsync();
        }

        public async Task<HBranch> FindAsync(Expression<Func<HBranch, bool>> predicate)
        {
            return await _dataContext.Branches.FirstOrDefaultAsync(predicate);
        }

        public async Task<HBranch> FindByOriginIdAsync(long originId)
        {
            return await _dataContext.Branches.FirstOrDefaultAsync(b => b.OriginId == originId);
        }

        public async Task AddAsync(HBranch item)
        {
            if (item.Id == 0)
            {
               await _dataContext.Branches.AddAsync(item);
            }
        }

        public void Delete(HBranch item)
        {
            _dataContext.Branches.Remove(item);
        }

        public async Task BulkUpdate(IList<HBranch> items)
        {
            using (var transaction = _dataContext.BeginTransaction())
            {
                foreach (var branch in items)
                {
                    if (!(branch.OriginId.HasValue && await FindByOriginIdAsync(branch.OriginId.Value) != null))
                    {
                        await _dataContext.Branches.AddAsync(branch);
                    }
                }

                await _dataContext.SaveChangesAsync();
                transaction.Commit();
            }
        }
    }
}