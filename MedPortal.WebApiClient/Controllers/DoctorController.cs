using AutoMapper;
using MedPortal.Data.Repositories;
using MedPortal.WebApiClient.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedPortal.Proxy.Controllers
{
    public class DoctorController : ControllerBase
    {
        private IDoctorRepository _clinicsRepository;
        private readonly IMapper _mapper;

        public DoctorController(IDoctorRepository clinicsRepository, IMapper mapper)
        {
            _clinicsRepository = clinicsRepository;
            _mapper = mapper;
        }
        [HttpGet("api/alldoctors")]
        public async Task<IActionResult> GetClinics()
        {
            var clinics = await _clinicsRepository.GetAsync();
            var result = _mapper.Map<List<DoctorModel>>(clinics);
            return Ok(result);
        }

        [HttpGet("api/doctors")]
        public async Task<IActionResult> GetClinics(string city, string speciality)
        {
            var doctors = await _clinicsRepository.FilterDoctorsAsync(city, speciality);
            var result = _mapper.Map<List<DoctorModel>>(doctors);
            return Ok(result);
        }
    }
}