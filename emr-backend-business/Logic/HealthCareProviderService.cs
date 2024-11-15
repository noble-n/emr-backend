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
    public class HealthCareProviderService : IHealthCareProviderService
    {
        private readonly ILogger<HealthCareProviderService> _logger;
        private readonly IAuthService _authService;
        private readonly IHealthCareProviderRepository _healthCareProviderRepository;

        public HealthCareProviderService(ILogger<HealthCareProviderService> logger, IAuthService authService, IHealthCareProviderRepository healthCareProviderRepository)
        {
            _logger = logger;
            _authService = authService;
            _healthCareProviderRepository = healthCareProviderRepository;
        }

        public async Task<ExecutedResult<string>> CreateHealthCareProvider(CreateHealthCareProviderVM request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
               
                request.CreatedByUserId = accessUser.data.UserId;
                var repoResponse = await _healthCareProviderRepository.CreateHealthCareProvider(request);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("HealthCareProvider created successfully.");
                return new ExecutedResult<string>() { responseMessage = "Health Care Provider created successfully.", responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: CreateHealthCareProvider(CreateHealthCareProviderVM  request ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> UpdateHealthCareProvider(UpdateHealthCareProviderVM request, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var healthCareProvider = await _healthCareProviderRepository.GetHealthCareProviderByID(request.HealthCareProviderId);

                if (healthCareProvider == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Not Found", responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }
                request.UpdatedByUserId = accessUser.data.UserId;
                var repoResponse = await _healthCareProviderRepository.UpdateHealthCareProvider(request);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("HealthCareProvider updated successfully.");
                return new ExecutedResult<string>() { responseMessage = "Health Care Provider updated successfully.", responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: UpdateHealthCareProvider(UpdateHealthCareProviderVM request ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<string>> DeleteHealthCareProvider(long HealthCareProviderID, string AccessKey, string RemoteIpAddress)
        {

            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<string>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var healthCareProvider = await _healthCareProviderRepository.GetHealthCareProviderByID(HealthCareProviderID);

                if (healthCareProvider == null)
                {
                    return new ExecutedResult<string>() { responseMessage = $"Not Found", responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };
                }

                var repoResponse = await _healthCareProviderRepository.DeleteHealthCareProvider(HealthCareProviderID, accessUser.data.UserId);

                if (!repoResponse.Contains("Success"))
                {
                    return new ExecutedResult<string>() { responseMessage = $"{repoResponse}", responseCode = ((int)ResponseCode.ProcessingError).ToString(), data = null };

                }

                _logger.LogInformation("Health Care Provider deleted successfully.");
                return new ExecutedResult<string>() { responseMessage = "Health Care Provider deleted successfully.", responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = null };

            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occurred: DeleteHealthCareProvider ==> {ex.Message}");
                return new ExecutedResult<string>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<IEnumerable<HealthCareProviderDTO>>> GetAllHealthCareProvider(string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<IEnumerable<HealthCareProviderDTO>>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {
                var healthCareProviders = await _healthCareProviderRepository.GetAllHealthCareProviders();

                if (healthCareProviders == null)
                {
                    return new ExecutedResult<IEnumerable<HealthCareProviderDTO>>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                _logger.LogInformation("Health Care Providers fetched successfully.");
                return new ExecutedResult<IEnumerable<HealthCareProviderDTO>>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = healthCareProviders };


            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception Occured: GetAllHealthCareProvider() ==> {ex.Message}");
                return new ExecutedResult<IEnumerable<HealthCareProviderDTO>>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        public async Task<ExecutedResult<HealthCareProviderDTO>> GetHealthCareProviderByID(long HealthCareProviderID, string AccessKey, string RemoteIpAddress)
        {
            var accessUser = await _authService.CheckUserAccess(AccessKey, RemoteIpAddress);
            if (accessUser.data == null)
            {
                return new ExecutedResult<HealthCareProviderDTO>() { responseMessage = $"Unathorized User", responseCode = ((int)ResponseCode.NotAuthenticated).ToString(), data = null };
            }
            try
            {

                var healthCareProvider = await _healthCareProviderRepository.GetHealthCareProviderByID(HealthCareProviderID);

                if (healthCareProvider == null)
                {
                    return new ExecutedResult<HealthCareProviderDTO>() { responseMessage = ResponseCode.NotFound.ToString(), responseCode = ((int)ResponseCode.NotFound).ToString(), data = null };

                }

                //update action performed into audit log here

                _logger.LogInformation("Health Care Provider fetched successfully.");
                return new ExecutedResult<HealthCareProviderDTO>() { responseMessage = ResponseCode.Ok.ToString(), responseCode = ((int)ResponseCode.Ok).ToString("D2"), data = healthCareProvider };

            }
            catch (Exception ex)
            {

                _logger.LogError($"Exception Occured: GetHealthCareProviderByID(long HealthCareProviderID) ==> {ex.Message}");
                return new ExecutedResult<HealthCareProviderDTO>() { responseMessage = "Unable to process the operation, kindly contact the support", responseCode = ((int)ResponseCode.Exception).ToString(), data = null };

            }
        }

        
    }
}
