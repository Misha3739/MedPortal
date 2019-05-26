using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;
using MedPortal.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace MedPortal.Proxy.Middleware {
	public class RequestResponseLoggingMiddleware {
		private readonly RequestDelegate _next;

		public RequestResponseLoggingMiddleware(RequestDelegate next) {
			_next = next;
		}

		public async Task Invoke(HttpContext context, IRepository<Log> logRepository, IUnitOfWork unitOfWork) {
			var log = new Log() {
				Ip = context.Connection.LocalIpAddress.ToString(),
				IncomeTime = DateTime.Now
			};

			//Copy a pointer to the original response body stream
			var originalBodyStream = context.Response.Body;


			//Create a new memory stream...
			using (var responseBody = new MemoryStream()) {
				//...and use that for the temporary response body
				context.Response.Body = responseBody;

				//Continue down the Middleware pipeline, eventually returning to this class
				await _next(context);

				log.StatusCode = context.Response.StatusCode;

				log.RequestedUrl = context.Request.Path.Value;


				using (StreamReader stream = new StreamReader(context.Request.Body)) {
					log.RequestBody = stream.ReadToEnd();
				}

				log.OutcomeTime = DateTime.Now;

				try {
					await logRepository.AddAsync(log);

					await unitOfWork.SaveChangesAsync();
				} catch (SqlException e) {
					Console.WriteLine(e);
				}


				//Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
				await responseBody.CopyToAsync(originalBodyStream);
			}
		}
	}
}