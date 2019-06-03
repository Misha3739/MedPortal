using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;

namespace MedPortal.Data.Repositories {
	public class ClinicsHighloadedRepoistory : HighloadedRepository<HClinic> {
		public ClinicsHighloadedRepoistory(IDataContext dataContext) : base(dataContext) {
		}

		protected override async Task BulkUpdateInternalAsync(IList<HClinic> items) {
			if (!items.Any()) {
				return;
			}

			await base.BulkUpdateInternalAsync(items);
			var originIds = items.Select(c => c.OriginId).ToList();

			using (var transaction = _dataContext.BeginTransaction()) {
				var clinicStations = items.SelectMany(clinic => clinic.Stations).ToList();
				Dictionary<long, long> clinicOriginIds = _dbSet.Where(c => originIds.Contains(c.OriginId))
					.Select(c => new {c.OriginId, c.Id}).ToDictionary(x => x.OriginId, x => x.Id);
				foreach (var clinic in items) {
					clinic.Id = clinicOriginIds[clinic.OriginId];
					foreach (var station in clinic.Stations) {
						station.ClinicId = clinic.Id;
					}
				}

				var clinicIds = clinicOriginIds.Select(c => c.Value).ToList();
				var existedClinicStations =
					_dataContext.ClinicStations.Where(c => clinicIds.Contains(c.ClinicId)).ToList();
				try {
					await _dataContext.BulkDeleteAsync(existedClinicStations);
					await _dataContext.BulkInsertAsync(clinicStations);

					transaction.Commit();
				} catch (Exception) {
					transaction.Rollback();
				}
			}
		}
	}
}