using System.Collections.Generic;

namespace MedPortal.Proxy.Data
{
    public class DoctorListResult
    {
        public IList<Doctor> DoctorList { get; set; }
        
        public long Total { get; set; }
    }
}