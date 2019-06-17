using System;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MedPortal.Proxy.Controllers
{
    public class ClinicController : ControllerBase
    {
        private IRepository<HClinic> _clinicsRepository;
        
        public ClinicController(IRepository<HClinic> clinicsRepository)
        {
            _clinicsRepository = clinicsRepository;


        }

        [HttpGet("api/clinics/{cityId}")]
        public async Task<IActionResult> GetClinics(long cityId)
        {
            var clinics = await _clinicsRepository.GetAsync(c => c.HCityId == cityId);
            
            return Ok(clinics);
        }
        
        
        
    }
}