using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Logging;
using MedPortal.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedPortal.Data.Repositories {
	public class ClinicsHighloadedRepoistory : HighloadedRepository<HClinic> {
		public ClinicsHighloadedRepoistory(IDataContext dataContext, ILogger logger) : base(dataContext, logger) {
		}

		protected override async Task BulkUpdateInternalAsync(IList<HClinic> items, bool assignIds) {
			if (!items.Any()) {
				return;
			}

			await base.BulkUpdateInternalAsync(items, true);

			using (var transaction = _dataContext.BeginTransaction()) {
				var clinicStations = items.SelectMany(clinic => clinic.Stations).ToList();
				foreach (var clinic in items) {
					foreach (var station in clinic.Stations) {
						station.ClinicId = clinic.Id;
					}
				}

				var clinicIds = items.Select(c => c.Id).ToList();
				var existedClinicStations =
					_dataContext.ClinicStations.Where(c => clinicIds.Contains(c.ClinicId)).ToList();
				try {
					await _dataContext.BulkDeleteAsync(existedClinicStations);
					await _dataContext.BulkInsertAsync(clinicStations);

					transaction.Commit();
				} catch (Exception e) {
                    _logger.LogError($"ClinicsHighloadedRepoistory. BulkUpdateInternalAsync: ", e);
                    transaction.Rollback();
				}
			}

            using (var transaction = _dataContext.BeginTransaction()) {
                var clinicSpecialities = items.SelectMany(clinic => clinic.Specialities).ToList();
                foreach (var clinic in items) {
                    foreach (var speciality in clinic.Specialities) {
                        speciality.ClinicId = clinic.Id;
                    }
                }

                var clinicIds = items.Select(c => c.Id).ToList();
                var existedClinicSpecialities =
                    _dataContext.ClinicSpecialities.Where(c => clinicIds.Contains(c.ClinicId)).ToList();
                try {
                    await _dataContext.BulkDeleteAsync(existedClinicSpecialities);
                    await _dataContext.BulkInsertAsync(clinicSpecialities);

                    transaction.Commit();
                }
                catch (Exception e) {
                    _logger.LogError($"ClinicsHighloadedRepoistory. BulkUpdateInternalAsync: ", e);
                    transaction.Rollback();
                }
            }
        }

        public override Task<List<HClinic>> GetAsync(Expression<Func<HClinic, bool>> predicate = null) {
            var query = _dbSet.AsNoTracking().Include(c => c.HCity).Include(c => c.HStreet);
            return predicate != null ? query.Where(predicate).ToListAsync() : query.ToListAsync();
        }
    }
}