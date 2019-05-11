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

        public SyncController(IHighloadedRepository<HCity> cityRepository)
        {
            _cityRepository = cityRepository;
        }

        [HttpPut("api/sync/clinics")]
        public async Task<IActionResult> SyncClinicData()
        {
            var data = await GetData<ClinicListResult>("clinic/list");
            var cities = (from clinic in data.ClinicList
                group clinic by (clinic.City, clinic.Stations?.FirstOrDefault()?.CityId) into city
                select new HCity(){ Name = city.Key.Item1, OriginId = city.Key.Item2 }).ToList();
                
            await _cityRepository.BulkUpdate(cities);
            return Ok();
        }
        
    }
}