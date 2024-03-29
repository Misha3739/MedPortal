using AutoMapper;
using MedPortal.Data.Business.SearchParameters;
using MedPortal.Data.Repositories;
using MedPortal.WebApiClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPortal.Proxy.Controllers
{
    public class DoctorController : ControllerBase
    {
        private IDoctorRepository _doctorsRepository;
        private readonly IMapper _mapper;

        public DoctorController(IDoctorRepository clinicsRepository, IMapper mapper)
        {
            _doctorsRepository = clinicsRepository;
            _mapper = mapper;
        }
        [HttpGet("api/alldoctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _doctorsRepository.GetAsync();
            var result = _mapper.Map<List<DoctorModel>>(doctors);
            return Ok(result);
        }

        [HttpGet("api/doctor/{alias}")]
        public async Task<IActionResult> GetDoctors(string alias)
        {
            var doctor = await _doctorsRepository.FindAsync(d => d.Alias == alias);
            var result = _mapper.Map<DoctorDetailsModel>(doctor);
            return Ok(result);
        }

        [HttpGet("api/doctors")]
        public async Task<IActionResult> GetDoctors(string city,
            string speciality,
            LocationTypeEnum? locationType = null,
            string location = null,
            int? inrange = null,
            double? latitude = null,
            double? longitude = null)
        {
            var locationSearchParameters = new LocationSearchParameters(city)
            {
                LocationType = locationType,
                Location = location,
                InRange = inrange,
                Latitude = latitude,
                Longitude = longitude
            };

            var doctors = await _doctorsRepository.FilterDoctorsAsync(locationSearchParameters, speciality);
            var result = new List<DoctorModel>();
            foreach (var doctor in doctors) {
                var doctorModel = _mapper.Map<DoctorModel>(doctor);
                var doctorSpecialities = doctor.Specialities.Select(s => s.Speciality).ToList();
                doctorModel.Specialities = _mapper.Map<List<DoctorSpecialitySearchModel>>(doctorSpecialities);
                var doctorClinics = doctor.Clinics.Select(s => s.Clinic).ToList();
                doctorModel.Clinics = _mapper.Map<List<ClinicModel>>(doctorClinics);
                result.Add(doctorModel);
            }
            return Ok(result);
        }
    }
}