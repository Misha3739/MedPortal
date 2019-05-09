using System.Threading.Tasks;
using MedPortal.Proxy.Data;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Extensions;

namespace MedPortal.Proxy.Controllers
{
    public class ClinicController : ControllerBase
    {
        private readonly IRestClient _restClient;

        private readonly string _baseAddress = "https://api.docdoc.ru/public/rest/1.0.12/";

        public ClinicController(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public async Task<IActionResult> GetClinics()
        {
            IRestRequest request = new RestRequest(_baseAddress + "clinic/list", Method.GET);
            
            var result =  _restClient.ExecuteAsync<ClinicListResult>(request, (response) => { });

            return result;
        }
    }
    
    
}