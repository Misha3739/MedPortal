﻿using System.Collections.Generic;

namespace MedPortal.WebApiClient.Models
{
    public class DoctorModel : DoctorSearchModel
    {
        public string Name { get; set; }

        //Original field is Img
        public string PhotoUrl { get; set; }

        public List<DoctorSpecialitySearchModel> Specialities { get; set; }
    }
}