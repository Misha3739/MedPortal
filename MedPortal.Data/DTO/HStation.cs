using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedPortal.Data.DTO
{
    public class HStation : HGeoObject
    {
		[Required]
		[MaxLength(6)]
		public string LineColor { get; set; }

		[Required]
		[MaxLength(100)]
		public string LineName { get; set; }

		public long CityId { get; set; }
        public HCity City { get; set; }
    }
}