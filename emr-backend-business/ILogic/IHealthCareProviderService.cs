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
    public interface IHealthCareProviderService
    {
        Task<ExecutedResult<string>> CreateHealthCareProvider(CreateHealthCareProviderVM request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> UpdateHealthCareProvider(UpdateHealthCareProviderVM request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> DeleteHealthCareProvider(long HealthCareProviderID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<HealthCareProviderDTO>> GetHealthCareProviderByID(long HealthCareProviderID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<HealthCareProviderDTO>>> GetAllHealthCareProvider(string AccessKey, string RemoteIpAddress);
    }
}
