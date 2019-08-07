using System.Collections.Generic;

namespace MedPortal.Proxy.Data
{
    public class Clinic
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string RewriteName { get; set; }
        public string Url { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public long StreetId { get; set; }
        public string Description { get; set; }
        public string House { get; set; }
        public string Phone { get; set; }
        public string Logo { get; set; }
        public long DistrictId { get; set; }
        public IList<Doctor> Doctors { get; set; }
        public IList<StationInfo> Stations { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public long ParentId { get; set; }
        public IList<long> BranchesId { get; set; }
        public bool OnlineRecordDoctor { get; set; }
        public bool IsActive { get; set; }
        public IList<Speciality> Specialities { get; set; }

        public Clinic()
        {
            Doctors = new List<Doctor>();
            BranchesId = new List<long>();
            Stations = new List<StationInfo>();
            Specialities = new List<Speciality>();
        }

    }
}