using System;
using System.Collections.Generic;
using System.Text;

namespace MedPortal.Data.Business.SearchParameters
{
    public class LocationSearchParameters
    {
        public LocationSearchParameters(string city)
        {
            City = city;
        }

        public string City { get; set; }

        public LocationTypeEnum? LocationType { get; set; }

        public string Location { get; set; }
    }
}
