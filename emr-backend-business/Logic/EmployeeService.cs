using emr_backend_business.ILogic;
using emr_backend_common.Communication;
using emr_backend_data.Enums;
using emr_backend_data.IRepository;
using emr_backend_data.RepoPayload;
using emr_backend_data.ViewModel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_business.Logic
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;
        private readonly IAuthService _authService;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService( ILogger<EmployeeService> logger, IAuthService authService, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _authService = authService;
            _employeeRepository = employeeRepository;
        }

        public async Task<ExecutedResult<string>> UpdateEmployee(UpdateEmployeeVM request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var employee = await _employeeRepository.GetEmployeeByID(request.EmployeeId);

                if (employee == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Not Found", responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }
                request.UpdatedByUserId = accessUser.data.UserId;
                var repoResponse = await _employeeRepository.UpdateEmployee(request);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("employee updated successfully.");
                return new ExecutedResult<string>() { responseMessage = "employee updated successfully.", responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: UpdateEmployee(UpdateEmployeeVM request ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }
        public async Task<ExecutedResult<string>> DeleteEmployee(long EmployeeID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var employee = await _employeeRepository.GetEmployeeByID(EmployeeID);

                if (employee == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Not Found", responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }

                var repoResponse = await _employeeRepository.DeleteEmployee(EmployeeID, accessUser.data.UserId);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Employee deleted successfully.");
                return new ExecutedResult<string>() { responseMessage = "Employee deleted successfully.", responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: DeleteEmployee ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<EmployeeDTO>>> GetAllEmployee(string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<EmployeeDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var employee = await _employeeRepository.GetAllEmployees(accessUser.data.HealthCareProviderId);

                if (employee == null)
                {
                    return new ExecutedResult<IEnumerable<EmployeeDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                _logger.LogInformation("Employee fetched successfully.");
                return new ExecutedResult<IEnumerable<EmployeeDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = employee };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllEmployees() ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<EmployeeDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<EmployeeDTO>> GetEmployeeByID(long EmployeeID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<EmployeeDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {

                var employee = await _employeeRepository.GetEmployeeByID(EmployeeID);

                if (employee == null)
                {
                    return new ExecutedResult<EmployeeDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Employee fetched successfully.");
                return new ExecutedResult<EmployeeDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = employee };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetEmployeeByID(long EmployeeID) ==> {ex.Message}");
                return new ExecutedResult<EmployeeDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<EmployeeDTO>> GetEmployeeByUserID(long UserID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<EmployeeDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {

                var employee = await _employeeRepository.GetEmployeeByUserID(UserID);

                if (employee == null)
                {
                    return new ExecutedResult<EmployeeDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Employee fetched successfully.");
                return new ExecutedResult<EmployeeDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = employee };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetEmployeeByUserID(long UserID) ==> {ex.Message}");
                return new ExecutedResult<EmployeeDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }
    }
}
