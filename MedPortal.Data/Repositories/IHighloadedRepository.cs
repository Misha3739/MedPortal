using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedPortal.Data.Repositories
{
    public interface IHighloadedRepository<T> : IRepository<T> where T : class, new()
    {
        Task BulkUpdate(IList<T> items);
    }
}