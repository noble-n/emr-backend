using emr_backend_business.AppCode;
using emr_backend_business.ILogic;
using emr_backend_common.Communication;
using emr_backend_data.Enums;
using emr_backend_data.IRepository;
using emr_backend_data.RepoPayload;
using emr_backend_data.Repository;
using emr_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_business.Logic
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthService> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly IJwtManager _jwtManager;




        public AuthService(IConfiguration config, ILogger<AuthService> logger, IAccountRepository accountRepository, IJwtManager jwtManager)
        {
            _config = config;
            _logger = logger;
            _accountRepository = accountRepository;
            _jwtManager = jwtManager;
        }


        public Task<ExecutedResult<string>> ChangePassword(ChangePasswordDTO payload)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecutedResult<UserDTO>> CheckUserAccess(string AccessToken, string IpAddress)
        {
            try
            {

                var userAccess = await _accountRepository.VerifyUser(AccessToken, IpAddress, DateTime.Now);
                if (!userAccess.Contains("Success"))
                {
                    _logger.LogError($"AuthService || (VerifyUser)  Unable to verify access =====>{userAccess}");
                    return new ExecutedResult<UserDTO>() { responseMessage = "Unathorized User", responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }
                var userData = await _accountRepository.FindUser(null, null, AccessToken);
                if (userData == null)
                {
                    _logger.LogError($"AuthService || (GetUserById)  Unable to get user details =====>");
                    return new ExecutedResult<UserDTO>() { responseMessage = ResponseCode.AuthorizationError.ToString(), responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
                }

                return new ExecutedResult<UserDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString(), data = userData };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception || UsersServices (CheckUserAccess)=====>{ex}");
                return new ExecutedResult<UserDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }

        public async Task<ExecutedResult<LoginResponse>> Login(LoginModel payload)
        {
            try
            {
                var user = await _accountRepository.GetUserByEmail(payload.Email);

                if (user == null)
                {
                    return new ExecutedResult<LoginResponse>() { responseMessage = "Incorrect email or password", responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }

                // Verify the hashed password
                var passwordIsValid = BCrypt.Net.BCrypt.Verify(payload.Password, user.PasswordHash);

                if (!passwordIsValid)
                {
                    return new ExecutedResult<LoginResponse>() { responseMessage = "Incorrect email or password", responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }
                var loginUser = await _accountRepository.LogInUser(user.Email);

                if (!loginUser.Contains("Success"))
                {
                    return new ExecutedResult<LoginResponse>() { responseMessage = "An error occurred", responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }
                var accessUserVm = new AccessUserVM();
                accessUserVm.PhoneNumber = user.PhoneNumber;
                accessUserVm.UserId = user.UserId;
                accessUserVm.LastName = user.LastName;
                accessUserVm.FirstName = user.FirstName;
                accessUserVm.Email = user.Email;

                // Generate JWT token
                var authResponse = await _jwtManager.GenerateJsonWebToken(accessUserVm);

                var loginResponse = new LoginResponse
                {
                    JwtToken = authResponse.JwtToken,
                    RefreshToken = authResponse.RefreshToken,
                    UserId = user.UserId,

                };
                return new ExecutedResult<LoginResponse>() { responseMessage = $"User Logged in Successfully.", responseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'), data = loginResponse };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception || AuthService (Login)=====>{ex}");
                return new ExecutedResult<LoginResponse>() { responseMessage = $"Unable to process the operation, kindly contact the support", responseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'), data = null };
            }

        }

        public async Task<ExecutedResult<string>> Logout(LogoutDTO payload)
        {
            try
            {
                var user = await _accountRepository.GetUserByEmail(payload.Email);

                if (user == null)
                {
                    return new ExecutedResult<string>() { responseMessage = "User not found", responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }
                var logoutUser = await _accountRepository.LogoutUser(user.Email);

                if (!logoutUser.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = "An error occurred", responseCode = ResponseCode.AuthorizationError.ToString("D").PadLeft(2, '0'), data = null };
                }
                return new ExecutedResult<string>() { responseMessage = $"User Logged out Successfully.", responseCode = ResponseCode.Ok.ToString("D").PadLeft(2, '0'), data = null };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception || AuthService (Logout)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = $"Unable to process the operation, kindly contact the support", responseCode = ResponseCode.Exception.ToString("D").PadLeft(2, '0'), data = null };
            }
        }

        public async Task<ExecutedResult<string>> SuperAdminSignUp(CreateSuperAdminUserVM payload)
        {
            try
            {

               
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
                var repoPayload = new CreateUserVM
                {
                    DateCreated = DateTime.Now,
                    FirstName = payload.FirstName,
                    LastName = payload.LastName,
                    MiddleName = payload.MiddleName,
                    Email = payload.Email,
                    PasswordHash = payload.PasswordHash,
                    PhoneNumber = payload.PhoneNumber,
                    RoleId = payload.RoleId,
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

               //Create employee record after successful user creation
               //var employeePayload = new CreateEmployeeVM
               //{
               //    HealthCareProviderId = payload.HealthCareProviderId, // Assume this is part of the payload
               //    UserId = createdUserId, // Assign the created user ID
               //    DepartmentId = payload.DepartmentId,
               //    FirstName = payload.FirstName,
               //    LastName = payload.LastName,
               //    MiddleName = payload.MiddleName,
               //    Gender = payload.Gender,
               //    Address = payload.Address,
               //    PhoneNumber = payload.PhoneNumber,
               //    RoleId = payload.RoleId,
               //    DateCreated = DateTime.Now,
               //};

               // string employeeResponse = await _employeeRepository.CreateEmployee(employeePayload);
               // if (!employeeResponse.Contains("Success"))
               // {
               //     return new ExecutedResult<string>
               //     {
               //         responseMessage = employeeResponse,
               //         responseCode = ((int)ResponseCode.ProcessingError).ToString(),
               //         data = null
               //     };
               // }


                return new ExecutedResult<string>() { responseMessage = "User created Successfully", responseCode = ((int)ResponseCode.Ok).ToString(), data = null };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UserService (CreateUser)=====>{ex}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };
            }
        }
    }
}
