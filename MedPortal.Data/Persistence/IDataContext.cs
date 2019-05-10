using MedPortal.Data.DTO;
using Microsoft.EntityFrameworkCore;

namespace MedPortal.Data.Persistence
{
    public interface IDataContext
    {
        DbSet<HBranch> Branches { get; set; }
        DbSet<HCity> Cities { get; set; }
        DbSet<HClinic> Clinics { get; set; }
        DbSet<HDistrict> Districs { get; set; }
        DbSet<HDoctor> Doctors { get; set; }
        DbSet<HSpeciality> Specialities { get; set; }
        DbSet<HStation> Stations { get; set; }
        DbSet<HStreet> Streets { get; set; }
        DbSet<HTelemed> Telemeds { get; set; }
    }
}