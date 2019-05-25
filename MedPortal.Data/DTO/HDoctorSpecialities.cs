using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HDoctorSpecialities : IEntity
	{
        [Key]
        public long Id { get; set; }
        
        public long DoctorId { get; set; }
        public HDoctor Doctor { get; set; }
        
        public long SpecialityId { get; set; }
        public HSpeciality Speciality { get; set; }
    }
}