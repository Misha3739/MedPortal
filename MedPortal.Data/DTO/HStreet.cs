using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO
{
    public class HStreet : HGeoObject
    {
	    public long CityId { get; set; }
	    public HCity City { get; set; }
	}
}