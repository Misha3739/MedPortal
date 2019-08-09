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
    public class LocationController : ControllerBase
    {
        private IRepository<HCity> _cityRepository;

        private IRepository<HDistrict> _districtRepository;

        private IRepository<HStation> _stationRepository;

        private IRepository<HStreet> _streetRepository;

        private readonly IMapper _mapper;

        public LocationController(
            IRepository<HCity> cityRepository, 
            IRepository<HDistrict> districtRepository, 
            IRepository<HStation> stationRepository, 
            IRepository<HStreet> streetRepository,
            IMapper mapper)
        {
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _stationRepository = stationRepository;
            _streetRepository = streetRepository;
            _mapper = mapper;
        }

        [HttpGet("/api/cities")]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _cityRepository.GetAsync();
            var result = _mapper.Map<List<CitySearchModel>>(cities);
            return Ok(result);
        }

        [HttpGet("/api/city")]
        public async Task<IActionResult> GetCity(double latitude, double longitude)
        {
            const double fluctuation = 0.20;
            var cities = await _cityRepository.GetAsync();
            var foundCity = cities.FirstOrDefault(c => 
                c.Latitude >= latitude - fluctuation &&
                c.Latitude <= latitude + fluctuation &&
                c.Longitude >= longitude - fluctuation &&
                c.Longitude <= longitude + fluctuation);
            SearchCityResultModel result = new SearchCityResultModel()
            {
                Cities = _mapper.Map<List<CitySearchModel>>(cities),
                Current = foundCity != null ? _mapper.Map<CitySearchModel>(foundCity) : null
            };
            return Ok(result);
        }

        [HttpGet("/api/locations/{city}")]
        public async Task<IActionResult> GetCityLocations(string city)
        {
            var hCity = await _cityRepository.FindAsync(c => c.Alias == city);
            if(hCity == null)
            {
                return BadRequest($"City with Alias = {city} does not exist");
            }
            else
            {
                var result = new List<GeoCategoryModel>();
                var districts = await _districtRepository.GetAsync(d => d.CityId == hCity.Id);
                result.Add(new GeoCategoryModel(GeoCategoryEnum.District, _mapper.Map<List<GeoSearchModel>>(districts)));
                var metroStations = await _stationRepository.GetAsync(d => d.CityId == hCity.Id);
                if(metroStations.Any())
                {
                    result.Add(new GeoCategoryModel(GeoCategoryEnum.MetroStation, _mapper.Map<List<GeoSearchModel>>(metroStations)));
                }
                var streets = await _streetRepository.GetAsync(d => d.CityId == hCity.Id);
                result.Add(new GeoCategoryModel(GeoCategoryEnum.Street, _mapper.Map<List<GeoSearchModel>>(streets)));
                
                return Ok(result);
            }
        }
    }
}
