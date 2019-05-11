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
        private readonly IHighloadedRepository<HBranch> _branchesRepository;

        public SyncController(IHighloadedRepository<HCity> cityRepository, IHighloadedRepository<HBranch> branchesRepository)
        {
            _cityRepository = cityRepository;
            _branchesRepository = branchesRepository;
        }

        [HttpPut("api/sync/clinics")]
        public async Task<IActionResult> SyncClinicData()
        {
            var data = await GetData<ClinicListResult>("clinic/list");
            var cities = (from clinic in data.ClinicList
                group clinic by (clinic.City, clinic.Stations?.FirstOrDefault()?.CityId) into city
                select new HCity(){ Name = city.Key.Item1, OriginId = city.Key.Item2 }).ToList();

            var stationsInfos = data.ClinicList.SelectMany(c => c.Stations);
               
            await _cityRepository.BulkUpdate(cities);
            
            var branches = (from stationInfo in stationsInfos
                group stationInfo by (stationInfo.LineName, stationInfo.LineColor,stationInfo.CityId, stationInfo.CityId) into branch
                select new HBranch()
                {
                    Name = branch.Key.Item1, 
                    LineColor = branch.Key.Item2,
                    CityId = _cityRepository.FindByOriginIdAsync(branch.Key.Item3).Result.Id,
                    OriginId = branch.Key.Item4,
                }).ToList();
            
            await _branchesRepository.BulkUpdate(branches);
            return Ok();
        }
        
    }
}