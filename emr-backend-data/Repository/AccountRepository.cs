using Dapper;
using emr_backend_data.IRepository;
using emr_backend_data.RepoPayload;
using emr_backend_data.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _dapper;
        private readonly ILogger<AccountRepository> _logger;
        private readonly IConfiguration _configuration;
        public AccountRepository(ILogger<AccountRepository> logger, IConfiguration configuration, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<dynamic> CreateUser(CreateUserVM request)
        {
            try
            {
                string pwd = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash, BCrypt.Net.BCrypt.GenerateSalt());
                var param = new DynamicParameters();
                //param.Add("@UserId", request.UserId);
                param.Add("@FirstName", request.FirstName);
                param.Add("@MiddleName", request.MiddleName);
                param.Add("@LastName", request.LastName);
                param.Add("@Email", request.Email);
                param.Add("@PhoneNumber", request.PhoneNumber);
                param.Add("@PasswordHash", pwd);
                param.Add("@DateCreated", request.DateCreated);
                param.Add("@RoleId", request.RoleId);
                param.Add("@HealthCareProviderId", request.HealthCareProviderId);


                param.Add("@UserID", dbType: DbType.Int32, direction: ParameterDirection.Output);

                dynamic response = await _dapper.Get<string>("sp_create_user", param, commandType: CommandType.StoredProcedure);

                // Retrieve the AssetID from the output parameter
                long userID = param.Get<int>("@UserID");


                return userID;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"AccountRepository => CreateUser ===> {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
        {
            try
            {
                var param = new DynamicParameters();

                dynamic response = await _dapper.GetAll<UserDTO>("sp_get_all_users", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"AccountRepository => GetUserByAllUsers || {ex}");
                throw;
            }
        }

        public async Task<UserDTO> GetUserByEmail(string Email)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Email", Email);

                dynamic response = await _dapper.Get<UserDTO>("sp_get_user_by_email", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"AccountRepository => GetUserByEmail || {ex}");
                throw;
            }

        }

        public async Task<UserDTO> GetUserByID(long userID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@userID", userID);
                return await _dapper.Get<UserDTO>("sp_get_user_by_id", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"AccountRepository => GetUserById || {ex}");
                throw;
            }
        }

        public async Task<string> LogInUser(string Email)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@Email", Email);

                return await _dapper.Get<string>("sp_login_user", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> LogInUser => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }
        }

        public async Task<string> LogoutUser(string Email)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Email", Email);

                return await _dapper.Get<string>("sp_log_out_user", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> LogoutUser => {ex}");
                return "An error occured, kindly contact support";
            }
        }

        public async Task<string> ChangePassword(long UserId, string defaultPassword)
        {
            try
            {
                string pwd = BCrypt.Net.BCrypt.HashPassword(defaultPassword, BCrypt.Net.BCrypt.GenerateSalt());
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                param.Add("@PasswordHashed", pwd);
                //param.Add("@CreatedByUserId", CreatedByUserId);
                param.Add("@DateCreated", DateTime.Now);

                return await _dapper.Get<string>("sp_change_user_password", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> ChangePassword => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }

        public async Task<string> UpdateRefreshToken(string RefreshToken, long UserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                param.Add("@RefreshToken", RefreshToken);

                return await _dapper.Get<string>("sp_update_refresh_token", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"AccountRepository => UpdateRefreshToken ===> {ex.Message}");
                throw;
            }
        }

        public async Task<string> VerifyUser(string Token, string LoggedInWithIPAddress, DateTime DateCreated)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@Token", Token);

                return await _dapper.Get<string>("sp_verify_user", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> VerifyUser => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }
        }
        public async Task<string> UpdateLoginActivity(long UserId, string Token, DateTime DateCreated)
        {
            try
            {
                var param = new DynamicParameters();

                param.Add("@UserId", UserId);
                param.Add("@Token", Token);
                param.Add("@DateCreated", DateCreated);
                return await _dapper.Get<string>("sp_update_login_activity", param, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository -> UpdateLoginActivity => {ex}");
                return "Unable to submit this detail, kindly contact support";
            }

        }
        public async Task<UserDTO> FindUser(long? UserId, string Email, string AccessToken)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserId", UserId);
                param.Add("@Email", Email);
                param.Add("@AccessToken", AccessToken);
                return await _dapper.Get<UserDTO>("sp_find_user", param, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError($"AccountRepository => FindUser || {ex}");
                return new UserDTO();
            }
        }

        public Task<UserDTO> FindUser(long? UserId)
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO> FindUser(string Email)
        {
            throw new NotImplementedException();
        }
    }
}
