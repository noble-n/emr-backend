using emr_backend_business.AppCode;
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
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_business.Logic
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthService _authService;
        private readonly IEmployeeRepository _employeeRepository;

        public UserService(IAccountRepository accountRepository, ILogger<UserService> logger, IAuthService authService,IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _authService = authService;
            _employeeRepository = employeeRepository;
        }

        public async Task<ExecutedResult<string>> CreateUser(CreateEmployeeVM payload, string AccessToken, string RemoteIpAddress)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessToken, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                bool isModelStateValidate = true;
                string validationMessage = "";

                if (string.IsNullOrEmpty(payload.FirstName))
                {
                    isModelStateValidate = false;
                    validationMessage += "First name is required";
                }
                if (payload.LastName == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Last name is required";
                }
                if (payload.Email == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Official email is required";
                }
                if (payload.PhoneNumber == null)
                {
                    isModelStateValidate = false;
                    validationMessage += "  || Phone number is required";
                }

                if (!isModelStateValidate)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{validationMessage}", responseCode = ((int)ResponseCode.ValidationError).ToString(), data = null };

                }
                var password = Utils.GenerateDefaultPassword(6);
                var repoPayload = new CreateUserVM
                {
                    DateCreated = DateTime.Now,
                    FirstName = payload.FirstName,
                    LastName = payload.LastName,
                    MiddleName = payload.MiddleName,
                    Email = payload.Email,
                    PasswordHash = password,
                    PhoneNumber = payload.PhoneNumber,
                    RoleId = payload.RoleId,
                    HealthCareProviderId = accessUser.data.HealthCareProviderId,
                };
                var repoResponse = await _accountRepository.CreateUser(repoPayload);
                if (repoResponse < 0)
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };
                }
                var createdUserId = repoResponse;

                //var auditLog = new AuditLogDto
                //{
                //    userId = accessUser.data.UserId,
                //    actionPerformed = "CreateBackOfficeUser",
                //    payload = JsonConvert.SerializeObject(payload),
                //    response = null,
                //    actionStatus = $"Successful",
                //    ipAddress = RemoteIpAddress
                //};
                //await _audit.LogActivity(auditLog);

                // Create employee record after successful user creation
                var employeePayload = new CreateEmployeeVM
                {
                    HealthCareProviderId = accessUser.data.HealthCareProviderId,
                    UserId = createdUserId,
                    DepartmentId = payload.DepartmentId,
                    FirstName = payload.FirstName,
                    LastName = payload.LastName,
                    MiddleName = payload.MiddleName,
                    Gender = payload.Gender,
                    Address = payload.Address,
                    PhoneNumber = payload.PhoneNumber,
                    RoleId = payload.RoleId,
                    DateCreated = DateTime.Now,
                    Email = payload.Email,
                    CreatedByUserId = accessUser.data.UserId,

                    
                };

                string employeeResponse = await _employeeRepository.CreateEmployee(employeePayload);
                if (!employeeResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>
                    {
                        responseMessage = employeeResponse,
                        responseCode = ((int)ResponseCode.ProcessingError).ToString(),
                        data = null
                    };
                }


                return new ExecutedResult<string>() { responseMessage = "Employee created Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserService (CreateUser)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<IEnumerable<UserDTO>>> GetAllUsers(string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<IEnumerable<UserDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var returnData = await _accountRepository.GetAllUsers();
                if (returnData == null)
                {
                    return new ExecutedResult<IEnumerable<UserDTO>>() { responseMessage = ((int)ResponseCode.NotFound).ToString().ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }

                return new ExecutedResult<IEnumerable<UserDTO>>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };

                
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserService (GetUsers)=====>{ex}");
                return new ExecutedResult<IEnumerable<UserDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<UserDTO>> GetUserByEmail(string email, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<UserDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var returnData = await _accountRepository.GetUserByEmail(email);
                if (returnData == null)
                {
                    return new ExecutedResult<UserDTO>() { responseMessage = ((int)ResponseCode.NotFound).ToString().ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<UserDTO>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserService (GetUserByEmail)=====>{ex}");
                return new ExecutedResult<UserDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<UserDTO>> GetUserById(long UserId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort)
        {
            try
            {
                var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
                if (accessUser.data == null)
                {
                    return new ExecutedResult<UserDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };

                }
                var returnData = await _accountRepository.GetUserByID(UserId);
                if (returnData == null)
                {
                    return new ExecutedResult<UserDTO>() { responseMessage = ((int)ResponseCode.NotFound).ToString().ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }
                return new ExecutedResult<UserDTO>() { responseMessage = ((int)ResponseCode.Ok).ToString().ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = returnData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserService (GetUserById)=====>{ex}");
                return new ExecutedResult<UserDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
