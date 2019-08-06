using System.Collections.Generic;

namespace MedPortal.WebApiClient.Models
{
    public class SearchCityResultModel
    {
        public List<CitySearchModel> Cities { get; set; }

        public CitySearchModel Current { get; set; }
    }
}
