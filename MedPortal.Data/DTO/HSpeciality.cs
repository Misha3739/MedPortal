using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HSpeciality
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [MaxLength(200)]
        public string Alias { get; set; }
    }
}