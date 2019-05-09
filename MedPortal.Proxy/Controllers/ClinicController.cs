using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MedPortal.Proxy.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Extensions;

namespace MedPortal.Proxy.Controllers
{
    public class ClinicController : ControllerBase
    {
        private readonly IRestClient _restClient;

        private readonly ILogger<ClinicController> _logger;

        private readonly string _baseAddress = "https://api.docdoc.ru/public/rest/1.0.12/";

        public ClinicController(IRestClient restClient, ILogger<ClinicController> logger)
        {
            _restClient = restClient;
            _logger = logger;
        }
        
        [HttpGet("api/clinics")]
        public async Task<IActionResult> GetClinics()
        {
            IRestRequest request = new RestRequest(_baseAddress + "clinic/list", Method.GET);
            
            var result = await _restClient.ExecuteGetTaskAsync<ClinicListResult>(request);
            
            _logger.LogInformation($"Requested to: {request.Resource}, Method: {request.Method}, Response code: {result.StatusCode}");
            
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"Request returned status code: {result.StatusCode}");
            }
            return Ok(result.Data);
        }
    }
    
    
}