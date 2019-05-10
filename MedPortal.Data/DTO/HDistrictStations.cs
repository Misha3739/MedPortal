using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HDistrictStations
    {
        [Key]
        public long Id { get; set; }
        
        public long StationId { get; set; }
        public HStation Station { get; set; }
        
        public long DistrictId { get; set; }
        public HDistrict District { get; set; }
    }
}