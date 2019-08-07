using AutoMapper;
using MedPortal.Data.DTO;
using MedPortal.Data.Repositories;
using MedPortal.WebApiClient.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedPortal.WebApiClient.Controllers
{
    public class SearchInfoController : ControllerBase
    {
        private readonly IRepository<HCity> _cityRepository;

        private readonly IRepository<HClinic> _clinicRepository;

        private readonly IRepository<HDoctor> _doctorRepository;

        private readonly IRepository<HSpeciality> _specialityRepository;

        private readonly IMapper _mapper;

        public SearchInfoController(
            IRepository<HCity> cityRepository,
            IRepository<HClinic> clinicRepository,
            IRepository<HDoctor> doctorRepository,
            IRepository<HSpeciality> specialityRepository,
            IMapper mapper)
        {
            _cityRepository = cityRepository;
            _clinicRepository = clinicRepository;
            _doctorRepository = doctorRepository;
            _specialityRepository = specialityRepository;
            _mapper = mapper;
        }

        [HttpGet("/api/searchItems")]
        public async Task<IActionResult> GetSearchItems()
        {
            List<SearchCategoryModel> result = InitSearchCategories(); 
            var clinics = await GetClinicSearchItems();
            result.First(c => c.Type == SearchCategoryEnum.Clinic).Items.AddRange(clinics);
            var doctors = await GetDoctorSearchItems();
            result.First(c => c.Type == SearchCategoryEnum.Doctor).Items.AddRange(doctors);
            var clinicSpecialities = await GetClinicSpecialitySearchItems();
            result.First(c => c.Type == SearchCategoryEnum.ClinicSpeciality).Items.AddRange(clinicSpecialities);
            var doctorSpecialities = await GetDoctorSpecialitySearchItems();
            result.First(c => c.Type == SearchCategoryEnum.DoctorSpeciality).Items.AddRange(doctorSpecialities);
            return Ok(result);
        }

        [HttpGet("/api/searchItems/{city}")]
        public async Task<IActionResult> GetSearchItems(string city)
        {
            var hCity = await _cityRepository.FindAsync(c => c.Alias == city);
            if (hCity == null) {
                return BadRequest($"City with alias = {city} does not exist!");
            } else
            {
                List<SearchCategoryModel> result = InitSearchCategories();
                var clinics = await GetClinicSearchItems(hCity);
                result.First(c => c.Type == SearchCategoryEnum.Clinic).Items.AddRange(clinics);
                var doctors = await GetDoctorSearchItems(hCity);
                result.First(c => c.Type == SearchCategoryEnum.Doctor).Items.AddRange(doctors);
                var clinicSpecialities = await GetClinicSpecialitySearchItems();
                result.First(c => c.Type == SearchCategoryEnum.ClinicSpeciality).Items.AddRange(clinicSpecialities);
                var doctorSpecialities = await GetDoctorSpecialitySearchItems();
                result.First(c => c.Type == SearchCategoryEnum.DoctorSpeciality).Items.AddRange(doctorSpecialities);
                return Ok(result);
            }
        }

        [HttpGet("/api/clinicspecialities")]
        public async Task<IActionResult> GetClinicSpecialities()
        {
            var specialities = await GetClinicSpecialitySearchItems();
            return Ok(specialities);
        }

        [HttpGet("/api/doctorspecialities")]
        public async Task<IActionResult> GetDoctorSpecialities()
        {
            var specialities = await GetDoctorSpecialities();
            return Ok(specialities);
        }

        private List<SearchCategoryModel> InitSearchCategories()
        {
            return new List<SearchCategoryModel>()
            {
                new SearchCategoryModel(SearchCategoryEnum.Clinic),
                new SearchCategoryModel(SearchCategoryEnum.Doctor),
                new SearchCategoryModel(SearchCategoryEnum.ClinicSpeciality),
                new SearchCategoryModel(SearchCategoryEnum.DoctorSpeciality)
            };
        }

        private async Task<List<ClinicSearchModel>> GetClinicSearchItems(HCity city = null)
        {
            var clinics = city == null ? await _clinicRepository.GetAsync() : await _clinicRepository.GetAsync(c => c.HCityId == city.Id);
            return _mapper.Map<List<ClinicSearchModel>>(clinics);
        }

        private async Task<List<DoctorSearchModel>> GetDoctorSearchItems(HCity city = null)
        {
            var doctors = city == null ? await _doctorRepository.GetAsync() : await _doctorRepository.GetAsync(c => c.CityId == city.Id);
            return _mapper.Map<List<DoctorSearchModel>>(doctors);
        }

        private async Task<List<DoctorSpecialitySearchModel>> GetDoctorSpecialitySearchItems()
        {
            var specialities = await _specialityRepository.GetAsync();
            return _mapper.Map<List<DoctorSpecialitySearchModel>>(specialities);
        }

        private async Task<List<ClinicSpecialitySearchModel>> GetClinicSpecialitySearchItems()
        {
            var specialities = await _specialityRepository.GetAsync();
            return _mapper.Map<List<ClinicSpecialitySearchModel>>(specialities);
        }
    }
}
