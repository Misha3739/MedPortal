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
            IRestRequest request = new RestRequest("clinic/list", Method.GET);
            
            var result = await RestClient.ExecuteGetTaskAsync<ClinicListResult>(request);
            
            Logger.LogInformation($"Requested to: {request.Resource}, Method: {request.Method}, Response code: {result.StatusCode}");
            
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"Request returned status code: {result.StatusCode}");
            }
            return Ok(result.Data);
        }
    }
}