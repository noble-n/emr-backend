using emr_backend_common.Communication;
using emr_backend_data.RepoPayload;
using emr_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_business.ILogic
{
    public interface IPatientService
    {
        Task<ExecutedResult<string>> CreatePatient(CreatePatientVM request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> UpdatePatient(UpdatePatientVM request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> DeletePatient(long PatientID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<PatientDTO>> GetPatientByID(long PatientID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<PatientDTO>>> GetAllPatient(string AccessKey, string RemoteIpAddress);
    }
}
