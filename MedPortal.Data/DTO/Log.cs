using System;
using System.ComponentModel.DataAnnotations;

namespace MedPortal.Data.DTO {
	public class Log : IEntity {
		[Key] public long Id { get; set; }

		public DateTime IncomeTime { get; set; }

		public DateTime OutcomeTime { get; set; }

		[MaxLength(200)] public string RequestedUrl { get; set; }
		[MaxLength(50)] public string Ip { get; set; }

		public int StatusCode { get; set; }

		public string RequestBody { get; set; }

		public string ExceptionInformation { get; set; }
	}
}