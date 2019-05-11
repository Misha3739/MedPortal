using System.Threading.Tasks;

namespace MedPortal.Data.Persistence
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}