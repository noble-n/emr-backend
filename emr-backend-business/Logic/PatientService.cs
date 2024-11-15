using emr_backend_business.ILogic;
using emr_backend_common.Communication;
using emr_backend_data.Enums;
using emr_backend_data.IRepository;
using emr_backend_data.RepoPayload;
using emr_backend_data.Repository;
using emr_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_business.Logic
{
    public class PatientService : IPatientService
    {
        private readonly ILogger<PatientService> _logger;
        private readonly IAuthService _authService;
        private readonly IPatientRepository _patientRepository;

        public PatientService(ILogger<PatientService> logger, IAuthService authService, IPatientRepository patientRepository)
        {
            _logger = logger;
            _authService = authService;
            _patientRepository = patientRepository;
        }
        public async Task<ExecutedResult<string>> CreatePatient(CreatePatientVM request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {

                request.CreatedByUserId = accessUser.data.UserId;
                request.HealthCareProviderId = accessUser.data.HealthCareProviderId;
                request.Age = DateTime.Now.Year - request.DateOfBirth.Year;


                var repoResponse = await _patientRepository.CreatePatient(request);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Patient created successfully.");
                return new ExecutedResult<string>() { responseMessage = "Patient created successfully.", responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: CreatePatient(CreatePatientVM  request ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> UpdatePatient(UpdatePatientVM request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var patient = await _patientRepository.GetPatientByID(request.PatientId);

                if (patient == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Not Found", responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }
                request.UpdatedByUserId = accessUser.data.UserId;
                request.Age = DateTime.Now.Year - request.DateOfBirth.Year ;

                var repoResponse = await _patientRepository.UpdatePatient(request);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Patient updated successfully.");
                return new ExecutedResult<string>() { responseMessage = "Patient updated successfully.", responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: UpdatePatient(UpdatePatientVM request ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> DeletePatient(long PatientID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var patient = await _patientRepository.GetPatientByID(PatientID);

                if (patient == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Not Found", responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }

                var repoResponse = await _patientRepository.DeletePatient(PatientID, accessUser.data.UserId);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Patient deleted successfully.");
                return new ExecutedResult<string>() { responseMessage = "Patient deleted successfully.", responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: DeletePatient ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<PatientDTO>>> GetAllPatient(string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<PatientDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var patients = await _patientRepository.GetAllPatients(accessUser.data.HealthCareProviderId);

                if (patients == null)
                {
                    return new ExecutedResult<IEnumerable<PatientDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                _logger.LogInformation("Patients fetched successfully.");
                return new ExecutedResult<IEnumerable<PatientDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = patients };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllPatient() ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<PatientDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<PatientDTO>> GetPatientByID(long PatientID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<PatientDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {

                var patient = await _patientRepository.GetPatientByID(PatientID);

                if (patient == null)
                {
                    return new ExecutedResult<PatientDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Patient fetched successfully.");
                return new ExecutedResult<PatientDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = patient };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetPatientByID(long PatientID) ==> {ex.Message}");
                return new ExecutedResult<PatientDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        
    }
}
