using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedPortal.Data.Persistence;
using Microsoft.Extensions.DependencyInjection;
using MedPortal.Data.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace MedPortal.ApiSyncService.Engine {
	public class EngineBase {
		private IRestClient _restClient;

		private ILogger _logger;

		private IUnitOfWork _unitOfWork;

		private IMapper _mapper;

		private readonly SemaphoreSlim _restClientSlim = new SemaphoreSlim(4);

		public IRestClient RestClient {
			get {
				_restClient = _restClient ?? DIProvider.ServiceProvider.GetService<IRestClient>();
				return _restClient;
			}
			set => _restClient = value;
		}

		public ILogger Logger {
			get => _logger ?? (_logger = DIProvider.ServiceProvider.GetService<ILogger>());
			set => _logger = value;
		}

		public IUnitOfWork UnitOfWork {
			get => _unitOfWork ?? (_unitOfWork = DIProvider.ServiceProvider.GetService<IUnitOfWork>());
			set => _unitOfWork = value;
		}

		public IMapper Mapper {
			get => _mapper ?? (_mapper = DIProvider.ServiceProvider.GetService<IMapper>());
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

			Logger.LogInfo(
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

            Logger.LogInfo (
				$"Requested to: {request.Resource}, Method: {request.Method}, Response code: {result.StatusCode}");

			if (result.StatusCode != HttpStatusCode.OK) {
				throw new HttpRequestException($"Request returned status code: {result.StatusCode}");
			}

			if (result.Data == null) {
				result.Data = JsonConvert.DeserializeObject<T>(result.Content);
				if (result.Data == null) {
					throw new FormatException($"Cannot deserialize result from {result.Content}");
				}
			}

			return result.Data;
		}
	}
}