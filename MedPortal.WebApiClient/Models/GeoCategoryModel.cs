using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPortal.WebApiClient.Models
{
    public class GeoCategoryModel
    {
        public GeoCategoryModel(GeoCategoryEnum type, List<GeoSearchModel> items)
        {
            Type = type;
            Items = items;
        }

        public GeoCategoryEnum Type { get; }

        public List<GeoSearchModel> Items { get; }
    }
}
