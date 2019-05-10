using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HClinicStations
    {
        [Key]
        public long Id { get; set; }
        
        public long ClinicId { get; set; }
        public HClinic Clinic { get; set; }
        
        public long StationId { get; set; }
        public HStation Station { get; set; }
    }
}