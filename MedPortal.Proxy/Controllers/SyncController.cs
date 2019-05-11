using System;
using System.Linq;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Repositories;
using MedPortal.Proxy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace MedPortal.Proxy.Controllers
{
    public class SyncController : MedPortalControllerBase
    {
        private readonly IHighloadedRepository<HCity> _cityRepository;
        private readonly IHighloadedRepository<HDistrict> _districtRepository;
        private readonly IHighloadedRepository<HBranch> _branchesRepository;

        public SyncController(IHighloadedRepository<HCity> cityRepository, IHighloadedRepository<HBranch> branchesRepository, IHighloadedRepository<HDistrict> districtRepository)
        {
            _cityRepository = cityRepository;
            _branchesRepository = branchesRepository;
            _districtRepository = districtRepository;
        }

        [HttpPut("api/sync/cities")]
        public async Task<IActionResult> SyncCities()
        {
            CityListResult cities = await GetData<CityListResult>("city");
            var hCities = cities.CityList.Select(c => Mapper.Map<City, HCity>(c)).ToList();
            await _cityRepository.BulkUpdate(hCities);
            return Ok();

        }
        
        [HttpPut("api/sync/stations")]
        public async Task<IActionResult> SyncStations()
        {
            CityListResult cities = await GetData<CityListResult>("branch");
            var hCities = cities.CityList.Select(c => Mapper.Map<City, HCity>(c)).ToList();
            await _cityRepository.BulkUpdate(hCities);
            return Ok();
        }
        
        [HttpPut("api/sync/districts")]
        public async Task<IActionResult> SyncDistrics()
        {
            DistrictListResult cities = await GetData<DistrictListResult>("district");
            var hDistrics = cities.DistrictList.Select(c => Mapper.Map<District, HDistrict>(c)).ToList();
            await _districtRepository.BulkUpdate(hDistrics);
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
               
            await _cityRepository.BulkUpdate(cities);

            cities = await _cityRepository.GetAsync();
            
            var branches = (from stationInfo in stationsInfos
                group stationInfo by (stationInfo.LineName, stationInfo.LineColor,stationInfo.CityId, stationInfo.CityId) into branch
                select new HBranch()
                {
                    Name = branch.Key.Item1, 
                    LineColor = branch.Key.Item2,
                    CityId = cities.First(c => c.OriginId == branch.Key.Item3).Id,
                    OriginId = branch.Key.Item4,
                }).ToList();
            
            await _branchesRepository.BulkUpdate(branches);
            return Ok();
        }
        
    }
}