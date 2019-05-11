using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HTelemed : IHEntity
    {
        [Key]
        public long Id { get; set; }
        
        public bool Chat { get; set; }
        public bool Phone { get; set; }
        
        public long HClinicId { get; set; }
        public HClinic HClinic { get; set; }
        
        public long OriginId { get; set; }
    }
}