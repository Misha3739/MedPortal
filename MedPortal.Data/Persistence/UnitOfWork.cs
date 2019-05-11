using System.Threading.Tasks;

namespace MedPortal.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDataContext _dataContext;

        public UnitOfWork(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dataContext.SaveChangesAsync();
        }
    }
}