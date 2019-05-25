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

namespace MedPortal.Proxy.Controllers {
	public class SyncController : MedPortalControllerBase {
		private readonly IHighloadedRepository<HCity> _cityRepository;
		private readonly IHighloadedRepository<HDistrict> _districtRepository;
		private readonly IHighloadedRepository<HStation> _stationsRepository;
		private readonly IHighloadedRepository<HSpeciality> _specialitiesRepository;

		public SyncController(
			IHighloadedRepository<HCity> cityRepository,
			IHighloadedRepository<HDistrict> districtRepository,
			IHighloadedRepository<HStation> stationsRepository,
			IHighloadedRepository<HSpeciality> specialitiesRepository) {
			_cityRepository = cityRepository;
			_districtRepository = districtRepository;
			_stationsRepository = stationsRepository;
			_specialitiesRepository = specialitiesRepository;
		}

		[HttpPut("api/sync/all")]
		public async Task<IActionResult> SyncAll() {
			await SyncCitiesInternalAsync();
			await SyncDistrictsInternalAsync();
			await SyncStationsInternalAsync();
			await SyncSpecialitiesInternalAsync();
			await SyncDistrictsInternalAsync();
			return Ok();
		}

		[HttpPut("api/sync/cities")]
		public async Task<IActionResult> SyncCities() {
			return await SyncCitiesInternalAsync();
		}

		[HttpPut("api/sync/stations")]
		public async Task<IActionResult> SyncStations() {
			return await SyncStationsInternalAsync();
		}

		[HttpPut("api/sync/districts")]
		public async Task<IActionResult> SyncDistrics() {
			return await SyncDistrictsInternalAsync();
		}

		[HttpPut("api/sync/specialities")]
		public async Task<IActionResult> SyncSpecialities() {
			return await SyncSpecialitiesInternalAsync();
		}

		[HttpPut("api/sync/clinics")]
		public async Task<IActionResult> SyncClinicData() {
			return await SyncClinicDataInternalAsync();
		}

		private async Task<IActionResult> SyncCitiesInternalAsync() {
			CityListResult cities = await GetDataWithPollingAsync<CityListResult>("city");
			var hCities = cities.CityList.Select(c => Mapper.Map<City, HCity>(c)).ToList();
			await _cityRepository.BulkUpdateAsync(hCities);
			return Ok();
		}

		private async Task<IActionResult> SyncStationsInternalAsync() {
			var cities = await _cityRepository.GetAsync();
			foreach (var city in cities) {
				try {
					var stations = await GetDataWithPollingAsync<StationsListResult>($"metro/city/{city.OriginId}");
					var hStations = stations.MetroList.Select(s => Mapper.Map<Station, HStation>(s)).ToList();
					hStations.ForEach(s => s.CityId = cities.First(c => c.OriginId == s.CityId).Id);
					await _stationsRepository.BulkUpdateAsync(hStations);
				} catch (HttpRequestException e) {
					Logger.LogError($"Cannot load stations for city \"{city.Name}\" due to request issues ");
				}
			}

			return Ok();
		}

		private async Task<IActionResult> SyncDistrictsInternalAsync() {
			DistrictListResult cities = await GetDataWithPollingAsync<DistrictListResult>("district");
			var hDistrics = cities.DistrictList.Select(c => Mapper.Map<District, HDistrict>(c)).ToList();
			await _districtRepository.BulkUpdateAsync(hDistrics);
			return Ok();
		}

		private async Task<IActionResult> SyncSpecialitiesInternalAsync() {
			SpecialityListResult specialities = await GetDataWithPollingAsync<SpecialityListResult>("speciality");
			var hSpecialities = specialities.SpecList.Select(c => Mapper.Map<Speciality, HSpeciality>(c)).ToList();
			await _specialitiesRepository.BulkUpdateAsync(hSpecialities);
			return Ok();
		}

		private async Task<IActionResult> SyncClinicDataInternalAsync() {
			return Ok();
		}
	}
}