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
    public class HealthCareProviderRepository : IHealthCareProviderRepository
    {
        private string _connectionString;
        private readonly IDapperGenericRepository _dapper;
        private readonly ILogger<HealthCareProviderRepository> _logger;
        private readonly IConfiguration _configuration;
        public HealthCareProviderRepository(ILogger<HealthCareProviderRepository> logger, IConfiguration configuration, IDapperGenericRepository dapper)
        {
            _logger = logger;
            _dapper = dapper;
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<dynamic> CreateHealthCareProvider(CreateHealthCareProviderVM request)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@HealthCareProviderName", request.HealthCareProviderName);
                param.Add("@Address", request.Address);
                param.Add("@PhoneNumber", request.PhoneNumber);
                param.Add("@State", request.State);
                param.Add("@City", request.City);
                param.Add("@Country", request.Country);
                param.Add("@Email", request.Email);
                param.Add("@CreatedByUserId", request.CreatedByUserId);

                // Call the stored procedure
                dynamic response = await _dapper.Get<string>("sp_create_healthcare_provider", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"HealthCareProviderRepository => CreateHealthCareProvider ===> {ex.Message}");
                throw;
            }
        }
        public async Task<string> UpdateHealthCareProvider(UpdateHealthCareProviderVM request)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@HealthCareProviderId", request.HealthCareProviderId);
                param.Add("@HealthCareProviderName", request.HealthCareProviderName);
                param.Add("@Address", request.Address);
                param.Add("@PhoneNumber", request.PhoneNumber);
                param.Add("@State", request.State);
                param.Add("@City", request.City);
                param.Add("@Country", request.Country);
                param.Add("@Email", request.Email);
                param.Add("@UpdatedByUserId", request.UpdatedByUserId);

                // Call the stored procedure
                dynamic response = await _dapper.Get<string>("sp_update_healthcare_provider", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"HealthCareProviderRepository => UpdateHealthCareProvider ===> {ex.Message}");
                throw;
            }
        }

        public async Task<string> DeleteHealthCareProvider(long HealthCareProviderID, long deletedByUserId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@HealthCareProviderID", HealthCareProviderID);
                param.Add("@deletedByUserId", deletedByUserId);

                // Call the stored procedure
                dynamic response = await _dapper.Get<string>("sp_delete_healthCareProvider", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                var err = ex.Message;
                _logger.LogError($"healthCareProviderRepository => DeleteHealthCareProvider ===> {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<HealthCareProviderDTO>> GetAllHealthCareProviders()
        {
            try
            {
                var param = new DynamicParameters();

                var response = await _dapper.GetAll<HealthCareProviderDTO>("Sp_get_all_healthCareProviders", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting healthCareProviders", ex);
                throw;
            }
        }

        public async Task<HealthCareProviderDTO> GetHealthCareProviderByID(long HealthCareProviderID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@HealthCareProviderID", HealthCareProviderID);

                var response = await _dapper.Get<HealthCareProviderDTO>("Sp_get_healthCareProvider_by_id", param, commandType: CommandType.StoredProcedure);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting healthCareProvider by id", ex);
                throw;
            }
        }

       
    }
}
