using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Repositories;
using MedPortal.Proxy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace MedPortal.Proxy.Controllers
{
    public class SyncController : MedPortalControllerBase
    {
        private readonly IHighloadedRepository<HCity> _cityRepository;
        private readonly IHighloadedRepository<HDistrict> _districtRepository;
        private readonly IHighloadedRepository<HStation> _stationsRepository;

        public SyncController(
	        IHighloadedRepository<HCity> cityRepository, 
	        IHighloadedRepository<HDistrict> districtRepository,
	        IHighloadedRepository<HStation> stationsRepository)
        {
            _cityRepository = cityRepository;
            _districtRepository = districtRepository;
            _stationsRepository = stationsRepository;
        }

        [HttpPut("api/sync/cities")]
        public async Task<IActionResult> SyncCities()
        {
            CityListResult cities = await GetData<CityListResult>("city");
            var hCities = cities.CityList.Select(c => Mapper.Map<City, HCity>(c)).ToList();
            await _cityRepository.BulkUpdateAsync(hCities);
            return Ok();

        }

		[HttpPut("api/sync/stations")]
		public async Task<IActionResult> SyncStations()
		{
			var cities = await _cityRepository.GetAsync();
			foreach (var city in cities)
			{
				try {
					var stations = await GetData<StationsListResult>($"metro/city/{city.OriginId}");
					var hStations = stations.MetroList.Select(s => Mapper.Map<Station, HStation>(s)).ToList();
					hStations.ForEach(s => s.CityId = cities.First(c => c.OriginId == s.CityId).Id);
					await _stationsRepository.BulkUpdateAsync(hStations);
				}
				catch (HttpRequestException e) {
					Logger.LogError($"Cannot load stations for city \"{city.Name}\" due to request issues ");
				}
				
			}

			return Ok();
		}

		[HttpPut("api/sync/districts")]
        public async Task<IActionResult> SyncDistrics()
        {
            DistrictListResult cities = await GetData<DistrictListResult>("district");
            var hDistrics = cities.DistrictList.Select(c => Mapper.Map<District, HDistrict>(c)).ToList();
            await _districtRepository.BulkUpdateAsync(hDistrics);
            return Ok();
        }

        [HttpPut("api/sync/clinics")]
        public async Task<IActionResult> SyncClinicData()
        {
            var data = await GetData<ClinicListResult>("clinic/list");
            var cities = (from clinic in data.ClinicList
                group clinic by (clinic.City, clinic.Stations?.FirstOrDefault()?.CityId ?? 0) into city
                select new HCity(){ Name = city.Key.Item1, OriginId = city.Key.Item2 }).ToList();

            var stationsInfos = data.ClinicList.SelectMany(c => c.Stations);
               
            await _cityRepository.BulkUpdateAsync(cities);

            cities = await _cityRepository.GetAsync();
            
          
            return Ok();
        }
        
    }

   
}