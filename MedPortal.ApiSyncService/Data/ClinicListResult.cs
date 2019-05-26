using System.Collections.Generic;

namespace MedPortal.Proxy.Data
{
    public class ClinicListResult
    {
        public IList<Clinic> ClinicList { get; set; }
        
        public long Total { get; set; }

        
    }
}