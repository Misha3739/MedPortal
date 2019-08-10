using MedPortal.Data.Business.SearchParameters;
using MedPortal.Data.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedPortal.Data.Repositories
{
    public interface IClinicRepository : IRepository<HClinic>
    {
        Task<List<HClinic>> FilterClinicsAsync(LocationSearchParameters locationSearchParameters, string speciality);
    }
}
