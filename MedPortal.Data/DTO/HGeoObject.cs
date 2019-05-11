using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public abstract class HGeoObject : IHEntity
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string Alias { get; set; }
        
        public double Longitude{ get; set; }
        public double Latitude{ get; set; }
        
        public long OriginId { get; set; }
    }
}