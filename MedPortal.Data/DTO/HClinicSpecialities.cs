using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HClinicSpecialities : IEntity
    {
        [Key]
        public long Id { get; set; }

        public long ClinicId { get; set; }
        public HClinic Clinic { get; set; }

        public long SpecialityId { get; set; }
        public HSpeciality Speciality { get; set; }
    }
}