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
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _dapper;
        private readonly ILogger<EmployeeRepository> _logger;
        private readonly IConfiguration _configuration;
        public EmployeeRepository(ILogger<EmployeeRepository> logger, IConfiguration configuration, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<dynamic> CreateEmployee(CreateEmployeeVM request)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@HealthCareProviderId", request.HealthCareProviderId);
                param.Add("@UserId", request.UserId);
                param.Add("@DepartmentId", request.DepartmentId);
                param.Add("@FirstName", request.FirstName);
                param.Add("@MiddleName", request.MiddleName);
                param.Add("@LastName", request.LastName);
                param.Add("@Gender", request.Gender);
                param.Add("@Address", request.Address);
                param.Add("@PhoneNumber", request.PhoneNumber);
                param.Add("@RoleId", request.RoleId);
                param.Add("@DateCreated", request.DateCreated);
                param.Add("@Email", request.Email);
                param.Add("@CreatedByUserId", request.CreatedByUserId);

                // Call the stored procedure
                dynamic response = await _dapper.Get<string>("sp_create_employee", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => CreateEmployee ===> {ex.Message}");
                throw;
            }
        }

        public async Task<string> UpdateEmployee(UpdateEmployeeVM request)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeId", request.EmployeeId);
                param.Add("@Email", request.Email);
                param.Add("@Address", request.Address);
                param.Add("@DepartmentId", request.DepartmentId);
                param.Add("@FirstName", request.FirstName);
                param.Add("@LastName", request.LastName);
                param.Add("@MiddleName", request.MiddleName);
                param.Add("@Gender", request.Gender);
                param.Add("@Address", request.Address);
                param.Add("@PhoneNumber", request.PhoneNumber);
                param.Add("@RoleId", request.RoleId);
                param.Add("@UpdatedByUserId", request.UpdatedByUserId);

                // Call the stored procedure
                dynamic response = await _dapper.Get<string>("sp_update_employee", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => UpdateEmployee ===> {ex.Message}");
                throw;
            }
        }

        public async Task<string> DeleteEmployee(long EmployeeID, long deletedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeID", EmployeeID);
                param.Add("@deletedByUserId", deletedByUserId);

                // Call the stored procedure
                dynamic response = await _dapper.Get<string>("sp_delete_employee", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"EmployeeRepository => DeleteEmployee ===> {ex.Message}");
                throw;
            }
        }

        public async Task<EmployeeDTO> GetEmployeeByID(long EmployeeID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@EmployeeID", EmployeeID);

                var response = await _dapper.Get<EmployeeDTO>("Sp_get_employee_by_id", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting employee by id", ex);
                throw;
            }
        }

        public async Task<EmployeeDTO> GetEmployeeByUserID(long UserID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserID", UserID);

                var response = await _dapper.Get<EmployeeDTO>("Sp_get_employee_by_user_id", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting employee by userId", ex);
                throw;
            }
        }

        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployees(long healthcareProviderId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@healthcareProviderId", healthcareProviderId);

                var response = await _dapper.GetAll<EmployeeDTO>("Sp_get_all_employees", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting employees", ex);
                throw;
            }
        }
    }
}
