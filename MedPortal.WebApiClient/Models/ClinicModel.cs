using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPortal.WebApiClient.Models
{
    public class ClinicModel : ClinicSearchModel
    {
        public string Logo { get; set; }

        public string Address { get; set; }

        public string Url { get; set; }
    }
}
