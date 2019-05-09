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
    }
}