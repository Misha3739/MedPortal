using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;

namespace MedPortal.Proxy.Controllers
{
    public class MedPortalControllerBase : ControllerBase
    {
        private IRestClient _restClient;

        private ILogger<MedPortalControllerBase> _logger;

        private IAuthenticator _authenticator;

        public IRestClient RestClient
        {
            get
            {
                _restClient = _restClient ?? HttpContext.RequestServices.GetService<IRestClient>();
                _restClient.Authenticator = _authenticator ?? (_authenticator =
                    HttpContext.RequestServices.GetService<IAuthenticator>());
                return _restClient;
            }
            set => _restClient = value;
        }

        public ILogger<MedPortalControllerBase> Logger
        {
            get => _logger ?? (_logger = HttpContext.RequestServices.GetService<ILogger<MedPortalControllerBase>>());
            set => _logger = value;
        }

        protected async Task<T> GetData<T>(string resource)
        {
            IRestRequest request = new RestRequest(resource, Method.GET);
            
            var result = await RestClient.ExecuteGetTaskAsync<T>(request);
            
            Logger.LogInformation($"Requested to: {request.Resource}, Method: {request.Method}, Response code: {result.StatusCode}");
            
            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpRequestException($"Request returned status code: {result.StatusCode}");
            }

            if (result.Data == null)
            {
                throw new FormatException($"Cannot deserialize result from {result.Content}");
            }

            return result.Data;
        }
    }
}