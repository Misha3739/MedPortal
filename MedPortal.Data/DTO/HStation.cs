using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedPortal.Data.DTO
{
    public class HStation : HGeoObject
    {
        public long BranchId { get; set; }
        public HBranch Branch { get; set; }
        
        public long CityId { get; set; }
        public HCity City { get; set; }
    }
}