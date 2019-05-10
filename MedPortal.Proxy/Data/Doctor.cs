using System.Collections.Generic;

namespace MedPortal.Proxy.Data
{
    public class Doctor
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Rating { get; set; }
        public long Sex { get; set; }
        public string Img { get; set; }
        public string ImgFormat { get; set; }
        public string AddPhoneNumber { get; set; }
        public string Category { get; set; }
        public string Degree { get; set; }
        public string Rank { get; set; }
        public string Description { get; set; }
        public string TextEducation { get; set; }
        public string TextDegree { get; set; }
        public string TextSpec { get; set; }
        public string TextCourse { get; set; }
        public string TextExperience { get; set; }
        public long ExperienceYear { get; set; }
        public long Price { get; set; }
        public long SpecialPrice { get; set; }
        public long Departure { get; set; }
        public IList<Clinic> Clinics { get; set; }
        
        public IList<ClinicsInfo> ClinicsInfo { get; set; }
        public string Alias { get; set; }
        public IList<DoctorSpeciality> Specialities { get; set; }
        public IList<Station> Stations { get; set; }
        public IList<Clinic> BookingClinics { get; set; }
        public bool IsActive { get; set; }
        public long KidsReception { get; set; }
        public long OpinionCount { get; set; }
        public string TextAbout { get; set; }
        public Telemed Telemed { get; set; }
        public string RatingReviewsLabel { get; set; }
        public bool IsExclusivePrice { get; set; }

        public Doctor()
        {
            Clinics = new List<Clinic>();
            ClinicsInfo = new List<ClinicsInfo>();
            Specialities = new List<DoctorSpeciality>();
            Stations = new List<Station>();
            BookingClinics = new List<Clinic>();
        }
    }
}