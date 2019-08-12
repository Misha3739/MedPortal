using System.Collections.Generic;
using System.Threading.Tasks;
using MedPortal.Data.Business.SearchParameters;
using MedPortal.Data.DTO;

namespace MedPortal.Data.Repositories
{
    public interface IDoctorRepository : IRepository<HDoctor>
    {
        Task<List<HDoctor>> FilterDoctorsAsync(LocationSearchParameters parameters, string speciality);
    }
}