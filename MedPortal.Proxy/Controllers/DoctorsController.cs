using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MedPortal.Proxy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace MedPortal.Proxy.Controllers
{
    public class DoctorsController : MedPortalControllerBase
    {
        [HttpGet("api/doctors")]
        public async Task<IActionResult> GetClinics()
        {
            var data = await GetDataWithPollingAsync<DoctorListResult>("doctor/list");
            return Ok(data);
        }
    }
}