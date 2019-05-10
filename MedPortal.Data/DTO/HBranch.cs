using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HBranch : HGeoObject
    {
        [MaxLength(6)]
        public string LineColor { get; set; }
    }
}