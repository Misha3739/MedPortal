using System.Collections.Generic;

namespace MedPortal.WebApiClient.Models
{
    public class DoctorModel : DoctorSearchModel
    {
        public long OriginId { get; set; }
        public string Name { get; set; }

        //Original field is Img
        public string PhotoUrl { get; set; }

        public int Experience { get; set; }
        public List<DoctorSpecialitySearchModel> Specialities { get; set; }

        public List<ClinicModel> Clinics { get; set; }
    }
}
