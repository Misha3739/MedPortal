using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HClinicDoctors
    {
        [Key]
        public long Id { get; set; }
        
        public long ClinicId { get; set; }
        public HClinic Clinic { get; set; }
        
        public long DoctorId { get; set; }
        public HDoctor Doctor { get; set; }
    }
}