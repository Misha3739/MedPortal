using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedPortal.Data.Business.SearchParameters;
using MedPortal.Data.DTO;
using MedPortal.Data.Repositories;
using MedPortal.WebApiClient.Models;
using Microsoft.AspNetCore.Mvc;

namespace MedPortal.Proxy.Controllers
{
    public class ClinicController : ControllerBase
    {
        private IClinicRepository _clinicsRepository;
        private readonly IMapper _mapper;

        public ClinicController(IClinicRepository clinicsRepository, IMapper mapper)
        {
            _clinicsRepository = clinicsRepository;
            _mapper = mapper;
        }

        [HttpGet("api/allclinics")]
        public async Task<IActionResult> GetClinics()
        {
            var clinics = await _clinicsRepository.GetAsync();
            var result = _mapper.Map<List<ClinicModel>>(clinics);
            return Ok(result);
        }

        [HttpGet("api/clinics")]
        public async Task<IActionResult> GetClinics(
            string city,
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
            var clinics = await _clinicsRepository.FilterClinicsAsync(locationSearchParameters, speciality);
            var result = _mapper.Map<List<ClinicModel>>(clinics);
            return Ok(result);
        }

        [HttpGet("api/clinic/{alias}")]
        public async Task<IActionResult> GetClinics(string alias) {
            var clinic = await _clinicsRepository.FindAsync(c => c.RewriteName == alias);
            var clinicModel = _mapper.Map<ClinicDetailsModel>(clinic);
            return Ok(clinicModel);
        }
    }
}