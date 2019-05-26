using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedPortal.Data.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;

namespace MedPortal.Proxy.Controllers {
	public class MedPortalControllerBase : ControllerBase {
		private IRestClient _restClient;

		private ILogger<MedPortalControllerBase> _logger;

		private IUnitOfWork _unitOfWork;

		private IMapper _mapper;

		private readonly SemaphoreSlim _restClientSlim = new SemaphoreSlim(1);

		public IRestClient RestClient {
			get {
				_restClient = _restClient ?? HttpContext.RequestServices.GetService<IRestClient>();
				return _restClient;
			}
			set => _restClient = value;
		}

		public ILogger<MedPortalControllerBase> Logger {
			get => _logger ?? (_logger = HttpContext.RequestServices.GetService<ILogger<MedPortalControllerBase>>());
			set => _logger = value;
		}

		public IUnitOfWork UnitOfWork {
			get => _unitOfWork ?? (_unitOfWork = HttpContext.RequestServices.GetService<IUnitOfWork>());
			set => _unitOfWork = value;
		}

		public IMapper Mapper {
			get => _mapper ?? (_mapper = HttpContext.RequestServices.GetService<IMapper>());
			set => _mapper = value;
		}

		protected async Task<T> GetDataAsync<T>(string resource) {
			IRestRequest request = new RestRequest(resource, Method.GET);

			IRestResponse<T> result = null;
			await _restClientSlim.WaitAsync();
			try {
				result = await RestClient.ExecuteGetTaskAsync<T>(request);
			} finally {
				_restClientSlim.Release();
			}

			Logger.LogInformation(
				$"Requested to: {request.Resource}, Method: {request.Method}, Response code: {result.StatusCode}");

			if (result.StatusCode != HttpStatusCode.OK) {
				throw new HttpRequestException($"Request returned status code: {result.StatusCode}");
			}

			if (result.Data == null) {
				throw new FormatException($"Cannot deserialize result from {result.Content}");
			}

			return result.Data;
		}

		protected async Task<T> GetDataWithPollingAsync<T>(string resource) {
			int timeout = 3 * 1000 * 10;
			CancellationTokenSource cts = new CancellationTokenSource();

			HttpStatusCode defaultHttpStatusCode = (HttpStatusCode) (-1);

			IRestRequest request = new RestRequest(resource, Method.GET);
			IRestResponse<T> result = new RestResponse<T>() {
				StatusCode = defaultHttpStatusCode
			};

			try {
				cts.CancelAfter(timeout);

				Task t = Task.Run(async () => {
					while (result.StatusCode == defaultHttpStatusCode ||
					       result.StatusCode == HttpStatusCode.Unauthorized) {
						await _restClientSlim.WaitAsync(cts.Token);
						try {
							result = await RestClient.ExecuteGetTaskAsync<T>(request, cts.Token);
						} finally {
							_restClientSlim.Release();
						}

						await Task.Delay(1000, cts.Token);
					}
				}, cts.Token);

				await Task.WhenAll(t);
			} catch (OperationCanceledException) {
			}

			Logger.LogInformation(
				$"Requested to: {request.Resource}, Method: {request.Method}, Response code: {result.StatusCode}");

			if (result.StatusCode != HttpStatusCode.OK) {
				throw new HttpRequestException($"Request returned status code: {result.StatusCode}");
			}

			if (result.Data == null) {
				throw new FormatException($"Cannot deserialize result from {result.Content}");
			}

			return result.Data;
		}
	}
}