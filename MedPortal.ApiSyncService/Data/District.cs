using System.Collections.Generic;

namespace MedPortal.Proxy.Data
{
    public class District {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
    }

    public class DistrictListResult
    {
        public List<District> DistrictList { get; set; }
    }
}