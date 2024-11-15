using emr_backend_data.RepoPayload;
using emr_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.IRepository
{
    public interface IPatientRepository
    {
        Task<dynamic> CreatePatient(CreatePatientVM request);
        Task<string> UpdatePatient(UpdatePatientVM request);
        Task<string> DeletePatient(long PatientID, long deletedByUserId);
        Task<PatientDTO> GetPatientByID(long PatientID);
        Task<IEnumerable<PatientDTO>> GetAllPatients(long healthcareProviderId);
    }
}
