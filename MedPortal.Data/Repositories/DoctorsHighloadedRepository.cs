using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedPortal.Data.DTO;
using MedPortal.Data.Persistence;

namespace MedPortal.Data.Repositories {
	public class DoctorsHighloadedRepoistory : HighloadedRepository<HDoctor> {

		public DoctorsHighloadedRepoistory(IDataContext dataContext) : base(dataContext) {
		}

		protected override async Task BulkUpdateInternalAsync(IList<HDoctor> items, bool assignIds) {
			if (!items.Any()) {
				return;
			}

			await base.BulkUpdateInternalAsync(items, true);

			var doctorsClinic = items.SelectMany(doctor => doctor.Clinics).ToList();

			var doctorsSpecialities = items.SelectMany(doctor => doctor.Specialities).ToList();

			foreach (var doctor in items) {
				foreach (var clinic in doctor.Clinics) {
					clinic.DoctorId = doctor.Id;
				}

				foreach (var specialities in doctor.Specialities) {
					specialities.DoctorId = doctor.Id;
				}
			}

			var doctorsIds = items.Select(c => c.Id).ToList();

			using (var transaction = _dataContext.BeginTransaction()) {

				var existedDoctorClinics =
					_dataContext.ClinicDoctors.Where(c => doctorsIds.Contains(c.DoctorId)).ToList();

				var existedDoctorSpecialities = _dataContext.DoctorSpecialities.Where(c => doctorsIds.Contains(c.DoctorId)).ToList();

				try {
					await _dataContext.BulkDeleteAsync(existedDoctorClinics);
					await _dataContext.BulkDeleteAsync(existedDoctorSpecialities);

					await _dataContext.BulkInsertAsync(doctorsClinic);
					await _dataContext.BulkInsertAsync(doctorsSpecialities);

					transaction.Commit();
				} catch (Exception) {
					transaction.Rollback();
				}
			}
		}
	}
}