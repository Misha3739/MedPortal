using MedPortal.Data.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedPortal.Data.Repositories
{
    public interface IClinicRepository : IRepository<HClinic>
    {
        Task<List<HClinic>> FilterClinicsAsync(string city, string speciality);
    }
}
