using System.Collections.Generic;
using System.Threading.Tasks;
using MedPortal.Data.DTO;

namespace MedPortal.Data.Repositories
{
    public interface IHighloadedRepository<T> : IRepository<T> where T : IHEntity
    {
        Task BulkUpdate(IList<T> items);
    }
}