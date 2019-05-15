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
    public class ClinicController : MedPortalControllerBase
    {
        [HttpGet("api/clinics")]
        public async Task<IActionResult> GetClinics()
        {
            var data = await GetDataWithPollingAsync<ClinicListResult>("clinic/list");
            return Ok(data);
        }
    }
}