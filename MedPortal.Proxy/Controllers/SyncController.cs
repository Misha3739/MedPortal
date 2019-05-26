using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
		private readonly IHighloadedRepository<HClinic> _clinicsRepository;
		private readonly IRepository<HClinicStations> _clinicStationsRepository;
		private readonly IHighloadedRepository<HStreet> _streetsRepository;

		private readonly ConcurrentDictionary<string, UpdateResult> _saved =
			new ConcurrentDictionary<string, UpdateResult>();

		public SyncController(
			IHighloadedRepository<HCity> cityRepository,
			IHighloadedRepository<HDistrict> districtRepository,
			IHighloadedRepository<HStation> stationsRepository,
			IHighloadedRepository<HSpeciality> specialitiesRepository,
			IRepository<HClinicStations> clinicStationsRepository,
			IHighloadedRepository<HClinic> clinicsRepository, IHighloadedRepository<HStreet> streetsRepository) {
			_cityRepository = cityRepository;
			_districtRepository = districtRepository;
			_stationsRepository = stationsRepository;
			_specialitiesRepository = specialitiesRepository;
			_clinicStationsRepository = clinicStationsRepository;
			_clinicsRepository = clinicsRepository;
			_streetsRepository = streetsRepository;
		}

		[HttpPut("api/sync/all")]
		public async Task<IActionResult> SyncAll() {
			try {
				await SyncCitiesInternalAsync();
				await SyncDistrictsInternalAsync();
				await SyncStreetsInternalAsync();
				await SyncStationsInternalAsync();
				await SyncSpecialitiesInternalAsync();
				await SyncClinicDataInternalAsync();
			} finally {
				foreach (var key in _saved.Keys) {
					Logger.Log(LogLevel.Critical, $"{key}  Total: {_saved[key].Total}, Saved: {_saved[key].Saved}");
				}
			}


			return Ok();
		}

		[HttpPut("api/sync/cities")]
		public async Task<IActionResult> SyncCities() {
			return await SyncCitiesInternalAsync();
		}

		[HttpPut("api/sync/streets")]
		public async Task<IActionResult> SyncStreets() {
			return await SyncStreetsInternalAsync();
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
			await ParallelQueriesAsync(SyncStationsPerCityAsync, "Stations");
			return Ok();
		}

		private async Task<IActionResult> SyncDistrictsInternalAsync() {
			await ParallelQueriesAsync(SyncDistrictsPerCityAsync, "Districts");
			return Ok();
		}

		private async Task<IActionResult> SyncStreetsInternalAsync() {
			await ParallelQueriesAsync(SyncStreetsPerCityAsync, "Streets");
			return Ok();
		}

		private async Task<IActionResult> SyncSpecialitiesInternalAsync() {
			SpecialityListResult specialities = await GetDataWithPollingAsync<SpecialityListResult>("speciality");
			var hSpecialities = specialities.SpecList.Select(c => Mapper.Map<Speciality, HSpeciality>(c)).ToList();
			await _specialitiesRepository.BulkUpdateAsync(hSpecialities);
			return Ok();
		}

		private async Task<IActionResult> SyncClinicDataInternalAsync() {
			await ParallelQueriesAsync(SyncClinicPerCityAsync, "Clinics");
			return Ok();
		}

		private async Task SyncDistrictsPerCityAsync(HCity city, string identifier) {
			DistrictListResult districts =
				await GetDataWithPollingAsync<DistrictListResult>($"district?city={city.OriginId}");
			_saved[identifier].Total += districts.DistrictList.Count;
			var hDistrics = districts.DistrictList.Select(c => Mapper.Map<District, HDistrict>(c)).ToList();
			hDistrics.ForEach(d => d.CityId = city.Id);
			await _districtRepository.BulkUpdateAsync(hDistrics);
			_saved[identifier].Saved += hDistrics.Count;
		}

		private async Task SyncStreetsPerCityAsync(HCity city, string identifier) {
			StreetListResult streets =
				await GetDataWithPollingAsync<StreetListResult>($"street?city={city.OriginId}");
			_saved[identifier].Total += streets.StreetList.Count;
			var hStreets = streets.StreetList.Select(c => Mapper.Map<Street, HStreet>(c)).ToList();
			hStreets.ForEach(s => s.CityId = city.Id);
			await _streetsRepository.BulkUpdateAsync(hStreets);
			_saved[identifier].Saved += hStreets.Count;
		}

		private async Task SyncStationsPerCityAsync(HCity city, string identifier) {
			var stations = await GetDataWithPollingAsync<StationsListResult>($"metro/city/{city.OriginId}");
			_saved[identifier].Total += stations.MetroList.Count;
			var hStations = stations.MetroList.Select(s => Mapper.Map<Station, HStation>(s)).ToList();
			hStations.ForEach(s => s.CityId = city.Id);
			await _stationsRepository.BulkUpdateAsync(hStations);
			_saved[identifier].Saved += hStations.Count;
		}

		private async Task SyncClinicPerCityAsync(HCity city, string identifier) {
			ClinicListResult clinics =
				await GetDataWithPollingAsync<ClinicListResult>($"clinic/list?city={city.OriginId}");
			_saved[identifier].Total += clinics.ClinicList.Count;
			var districts = await _districtRepository.GetAsync(d => d.CityId == city.Id);
			var streets = await _streetsRepository.GetAsync(d => d.CityId == city.Id);
			int bulkSize = 20;
			var bulkList = new List<HClinic>();
			for (int i = 0; i < (clinics.ClinicList.Count / bulkSize) + 1; i++) {
				try {
					bulkList.Clear();
					foreach (var clinic in clinics.ClinicList.Skip(bulkSize * i).Take(bulkSize).ToList()) {
						var hClinic = Mapper.Map<Clinic, HClinic>(clinic);
						hClinic.HCityId = city.Id;
						hClinic.HStreetId = streets.First(s => s.OriginId == clinic.StreetId).Id;
						hClinic.HDistrictId =
							districts.FirstOrDefault(s => s.OriginId == clinic.DistrictId)?.Id;
						hClinic.ParentId =
							(await _clinicsRepository.FindAsync(s => s.OriginId == clinic.ParentId))
							?.Id;
						bulkList.Add(hClinic);
					}

					await _clinicsRepository.BulkUpdateAsync(bulkList);
					_saved[identifier].Saved += bulkList.Count;
				} catch (Exception e) {
					Logger.Log(LogLevel.Error, $"Error on saving Clinics for city: {city.Name}:{e}");
				}
			}
		}

		private async Task ParallelQueriesAsync(Func<HCity, string, Task> action, string identifier) {
			AddItemToDictionary(identifier);
			var cities = await _cityRepository.GetAsync();

			List<Task> tasks = new List<Task>();

			foreach (var city in cities) {
				tasks.Add(Task.Run(async () => {
					try {
						await action(city, identifier);
					} catch (Exception e) {
						Logger.Log(LogLevel.Error, $"Error on saving {identifier} for city: {city.Name}:{e}");
					}
				}));
				await Task.WhenAll(tasks);
			}
		}

		private void AddItemToDictionary(string key) {
			if (!_saved.ContainsKey(key)) {
				_saved.TryAdd(key, new UpdateResult());
			}
		}
	}
}