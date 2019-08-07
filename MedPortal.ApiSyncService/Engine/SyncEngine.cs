using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using MedPortal.Data.Repositories;
using MedPortal.Proxy.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MedPortal.ApiSyncService.Engine
{
	public class SyncEngine : EngineBase
	{
		private readonly IHighloadedRepository<HCity> _cityRepository;
		private readonly IHighloadedRepository<HSpeciality> _specialitiesRepository;
		private readonly IUnitOfWork _unitOfWork;

		private readonly ConcurrentDictionary<string, UpdateResult> _saved =
			new ConcurrentDictionary<string, UpdateResult>();

		private readonly object _dictionaryLocker = new object();

		public SyncEngine(
			IHighloadedRepository<HCity> cityRepository,
			IHighloadedRepository<HSpeciality> specialitiesRepository,
			IUnitOfWork unitOfWork) {
			_cityRepository = cityRepository;
			_specialitiesRepository = specialitiesRepository;
			this._unitOfWork = unitOfWork;
		}

		public async Task SyncAll() {
			try {
                await SyncCitiesInternalAsync();
                await SyncDistrictsInternalAsync();
                await SyncStreetsInternalAsync();
                await SyncStationsInternalAsync();
                await SyncSpecialitiesInternalAsync();
                await SyncClinicDataInternalAsync();
                await SyncDoctorsAsync();
            } finally {
				foreach (var key in _saved.Keys) {
					Logger.LogInfo($"{key}  Total: {_saved[key].Total}, Saved: {_saved[key].Saved}");
				}
			}
		}

		private async Task SyncDoctorsAsync() {
			await SplitQueriesInParallelAsync(SyncDoctorsPerCityAsync, "Doctors");
		}

		private async Task SyncCitiesInternalAsync() {
			CityListResult cities = await GetDataWithPollingAsync<CityListResult>("city");
            Logger.LogInfo($"SyncEngine. Received cities : {cities.CityList.Count}");
            var hCities = cities.CityList.Select(c => Mapper.Map<City, HCity>(c)).ToList();
			await _cityRepository.BulkUpdateAsync(hCities);
		}

		private async Task SyncStationsInternalAsync() {
			await SplitQueriesInParallelAsync(SyncStationsPerCityAsync, "Stations");
		}

		private async Task SyncDistrictsInternalAsync() {
			await SplitQueriesInParallelAsync(SyncDistrictsPerCityAsync, "Districts");
		}

		private async Task SyncStreetsInternalAsync() {
			await SplitQueriesInParallelAsync(SyncStreetsPerCityAsync, "Streets");
		}

		private async Task SyncSpecialitiesInternalAsync() {
			SpecialityListResult specialities = await GetDataWithPollingAsync<SpecialityListResult>("speciality");
            Logger.LogInfo($"SyncEngine. Received specialitiws : {specialities.SpecList.Count}");
            var hSpecialities = specialities.SpecList.Select(c => Mapper.Map<Speciality, HSpeciality>(c)).ToList();
			await _specialitiesRepository.BulkUpdateAsync(hSpecialities);
		}

		private async Task SyncClinicDataInternalAsync() {
			await SplitQueriesInParallelAsync(SyncClinicPerCityAsync, "Clinics");
		}

		private async Task SyncDistrictsPerCityAsync(HCity city, string identifier) {
			DistrictListResult districts =
				await GetDataWithPollingAsync<DistrictListResult>($"district?city={city.OriginId}");
            Logger.LogInfo($"SyncEngine. Received districts per {city.Name} : {districts.DistrictList.Count}");
            IncrementDictionary(identifier, 0, districts.DistrictList.Count);
			var hDistrics = districts.DistrictList.Select(c => Mapper.Map<District, HDistrict>(c)).ToList();
			hDistrics.ForEach(d => d.CityId = city.Id);
			var districtsRepository = DIProvider.ServiceProvider.GetService<IHighloadedRepository<HDistrict>>();
			await districtsRepository.BulkUpdateAsync(hDistrics);
			IncrementDictionary(identifier, hDistrics.Count, 0);
		}

		private async Task SyncStreetsPerCityAsync(HCity city, string identifier) {
			StreetListResult streets =
				await GetDataWithPollingAsync<StreetListResult>($"street?city={city.OriginId}");
            Logger.LogInfo($"SyncEngine. Received streets per {city.Name} : {streets.StreetList.Count}");
            IncrementDictionary(identifier, 0, streets.StreetList.Count);
			var hStreets = streets.StreetList.Select(c => Mapper.Map<Street, HStreet>(c)).ToList();
			hStreets.ForEach(s => s.CityId = city.Id);
			var streetsRepository = DIProvider.ServiceProvider.GetService<IHighloadedRepository<HStreet>>();
			await streetsRepository.BulkUpdateAsync(hStreets);
			IncrementDictionary(identifier, hStreets.Count, 0);
		}

		private async Task SyncStationsPerCityAsync(HCity city, string identifier) {
			var stations = await GetDataWithPollingAsync<StationsListResult>($"metro/city/{city.OriginId}");
            Logger.LogInfo($"SyncEngine. Received stations per {city.Name} : {stations.MetroList.Count}");
            IncrementDictionary(identifier, 0, stations.MetroList.Count);
			var hStations = stations.MetroList.Select(s => Mapper.Map<Station, HStation>(s)).ToList();
			hStations.ForEach(s => s.CityId = city.Id);
			var stationsRepository = DIProvider.ServiceProvider.GetService<IHighloadedRepository<HStation>>();
			await stationsRepository.BulkUpdateAsync(hStations);
			IncrementDictionary(identifier, hStations.Count, 0);
		}

		private async Task SyncClinicPerCityAsync(HCity city, string identifier) {
			var districtsRepository = DIProvider.ServiceProvider.GetService<IHighloadedRepository<HDistrict>>();
			var clinicsRepository = DIProvider.ServiceProvider.GetService<IHighloadedRepository<HClinic>>();
			var streetsRepository = DIProvider.ServiceProvider.GetService<IHighloadedRepository<HStreet>>();
			var stationsRepository = DIProvider.ServiceProvider.GetService<IRepository<HStation>>();
            var specialitiesRepository = DIProvider.ServiceProvider.GetService<IRepository<HSpeciality>>();

            var stations = await stationsRepository.GetAsync();

			var districts = await districtsRepository.GetAsync(d => d.CityId == city.Id);
			var streets = await streetsRepository.GetAsync(d => d.CityId == city.Id);

            var specialities = await specialitiesRepository.GetAsync();

            var noneDistrict = districtsRepository.FindAsync(d => d.Name == "NONE");
			var noneStreet = streetsRepository.FindAsync(d => d.Name == "NONE");

			ClinicListResult clinics = new ClinicListResult();
			int bulkSize = 100;
			var bulkList = new List<HClinic>();
			int i = 0;
			while (clinics.ClinicList == null || clinics.ClinicList.Any()) {
				try {
					clinics = await GetDataWithPollingAsync<ClinicListResult>(
						$"clinic/list?city={city.OriginId}&start={i * bulkSize}&count={bulkSize}");
					IncrementDictionary(identifier, 0, clinics.ClinicList.Count);

					foreach (var clinic in clinics.ClinicList) {
						var hClinic = Mapper.Map<Clinic, HClinic>(clinic);
						hClinic.HCityId = city.Id;
						hClinic.HStreetId = streets.FirstOrDefault(s => s.OriginId == clinic.StreetId)?.Id ?? noneStreet.Id;
						hClinic.HDistrictId =
							districts.FirstOrDefault(s => s.OriginId == clinic.DistrictId)?.Id ?? noneDistrict.Id;

						var stationIds = clinic.Stations.Select(s => s.Id).ToList();
						hClinic.Stations = stations.Where(s => stationIds.Contains(s.OriginId)).Select(s => new HClinicStations() {
							StationId = s.Id,
							Clinic = hClinic
						}).ToList();

                        var specialityIds = clinic.Specialities.Select(s => s.Id).ToList();
                        hClinic.Specialities = specialities.Where(s => specialityIds.Contains(s.OriginId)).Select(s => new HClinicSpecialities()
                        {
                            SpecialityId = s.Id,
                            Clinic = hClinic
                        }).ToList();

                        hClinic.ParentId =
							(await clinicsRepository.FindAsync(s => s.OriginId == clinic.ParentId))
							?.Id;
						bulkList.Add(hClinic);
					}

					await clinicsRepository.BulkUpdateAsync(bulkList);
					IncrementDictionary(identifier, bulkList.Count);
				} catch (Exception e) {
					Logger.LogError($"Error on saving Clinics for city: {city.Name}. Position: {i * bulkSize}", e);
				}

				bulkList.Clear();
				i++;
			}
		}

		private async Task SplitQueriesInParallelAsync(Func<HCity, string, Task> action, string identifier) {
			AddItemToDictionary(identifier);
			var cities = await _cityRepository.GetAsync();
			var tasks = new List<Task>();
			foreach (var city in cities) {
				tasks.Add(Task.Run(async () => {
					try {
						await action(city, identifier);
					} catch (Exception e) {
						Logger.LogError($"Error on saving {identifier} for city: {city.Name}", e);
					}
				}));
			}

			await Task.WhenAll(tasks);
		}

		private async Task SyncDoctorsPerCityAsync(HCity city, string identifier) {
			var doctors = await GetDataWithPollingAsync<DoctorListResult>($"doctor/list?city={city.OriginId}");
            Logger.LogInfo($"SyncEngine. Received doctors per {city.Name} : {doctors.Total}");
			IncrementDictionary(identifier, 0, doctors.DoctorList.Count);

			var specialitiesRepository = DIProvider.ServiceProvider.GetService<IRepository<HSpeciality>>();

			var clinicRepository = DIProvider.ServiceProvider.GetService<IRepository<HClinic>>();

			List<HDoctor> hDoctorsList = new List<HDoctor>();

			var specialitiesList = await specialitiesRepository.GetAsync();

			foreach (var doctor in doctors.DoctorList) {

				var hDoctor = Mapper.Map<Doctor, HDoctor>(doctor);


				hDoctor.Clinics = (await clinicRepository.GetAsync(c => doctor.Clinics.Contains(c.OriginId)))

					.Select(c => new HClinicDoctors() {
						ClinicId = c.Id
					}).ToList();

				var specialitiesIDs = doctor.Specialities.Select(c => c.Id).ToList();

				hDoctor.Specialities = specialitiesList.Where(c => specialitiesIDs.Contains(c.OriginId)).Select(c => new HDoctorSpecialities() {
					SpecialityId = c.Id
				}).ToList();

				hDoctor.CityId = city.Id;

				hDoctorsList.Add(hDoctor);

			}

			var doctorsRepository = DIProvider.ServiceProvider.GetService<IHighloadedRepository<HDoctor>>();
			await doctorsRepository.BulkUpdateAsync(hDoctorsList);
			IncrementDictionary(identifier, hDoctorsList.Count, 0);
		}

		private void AddItemToDictionary(string key) {
			if (!_saved.ContainsKey(key)) {
				_saved.TryAdd(key, new UpdateResult());
			}
		}

		private void IncrementDictionary(string key, int saved = 0, int total = 0) {
			lock (_dictionaryLocker) {
				_saved[key].Saved += saved;
				_saved[key].Total += total;
			}
		}
	}
}