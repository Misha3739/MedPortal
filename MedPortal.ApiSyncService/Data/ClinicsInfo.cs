using System.Collections.Generic;

namespace MedPortal.Proxy.Data
{
    public class ClinicsInfo
    {
        public long ClinicId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public bool RequestFormSurname { get; set; }
        public bool RequestFormBirthday { get; set; }
        public bool Recommend { get; set; }

    }
}