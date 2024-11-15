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
    public class PatientRepository : IPatientRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _dapper;
        private readonly ILogger<PatientRepository> _logger;
        private readonly IConfiguration _configuration;
        public PatientRepository(ILogger<PatientRepository> logger, IConfiguration configuration, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<dynamic> CreatePatient(CreatePatientVM request)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@HealthCareProviderId", request.HealthCareProviderId);
                param.Add("@FirstName", request.FirstName);
                param.Add("@MiddleName", request.MiddleName);
                param.Add("@LastName", request.LastName);
                param.Add("@Gender", request.Gender);
                param.Add("@Address", request.Address);
                param.Add("@PhoneNumber", request.PhoneNumber);
                param.Add("@DateOfBirth", request.DateOfBirth.Date);
                param.Add("@Age", request.Age);
                param.Add("@Email", request.Email);
                param.Add("@CreatedByUserId", request.CreatedByUserId);

                // Call the stored procedure
                dynamic response = await _dapper.Get<string>("sp_create_patient", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"PatientRepository => CreatePatient ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> UpdatePatient(UpdatePatientVM request)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PatientId", request.PatientId);
                param.Add("@FirstName", request.FirstName);
                param.Add("@MiddleName", request.MiddleName);
                param.Add("@LastName", request.LastName);
                param.Add("@Gender", request.Gender);
                param.Add("@Address", request.Address);
                param.Add("@PhoneNumber", request.PhoneNumber);
                param.Add("@DateOfBirth", request.DateOfBirth.Date);
                param.Add("@Age", request.Age);
                param.Add("@Email", request.Email);
                param.Add("@UpdatedByUserId", request.UpdatedByUserId);

                // Call the stored procedure
                dynamic response = await _dapper.Get<string>("sp_update_patient", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"PatientRepository => UpdatePatient ===> {ex.Message}");
                throw;
            }
        }

        public async Task<string> DeletePatient(long PatientID, long deletedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PatientID", PatientID);
                param.Add("@deletedByUserId", deletedByUserId);

                // Call the stored procedure
                dynamic response = await _dapper.Get<string>("sp_delete_patient", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"PatientRepository => DeletePatient ===> {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<PatientDTO>> GetAllPatients(long healthcareProviderId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@healthcareProviderId", healthcareProviderId);

                var response = await _dapper.GetAll<PatientDTO>("Sp_get_all_patients", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Patients", ex);
                throw;
            }
        }

        public async Task<PatientDTO> GetPatientByID(long PatientID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@PatientID", PatientID);

                var response = await _dapper.Get<PatientDTO>("Sp_get_patient_by_id", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting Patient by id", ex);
                throw;
            }
        }

        
    }
}
