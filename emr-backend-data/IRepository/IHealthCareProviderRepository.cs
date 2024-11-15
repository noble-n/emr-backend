using emr_backend_data.RepoPayload;
using emr_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.IRepository
{
    public interface IHealthCareProviderRepository
    {
        Task<dynamic> CreateHealthCareProvider(CreateHealthCareProviderVM request);
        Task<string> UpdateHealthCareProvider(UpdateHealthCareProviderVM request);
        Task<string> DeleteHealthCareProvider(long HealthCareProviderID, long deletedByUserId);
        Task<HealthCareProviderDTO> GetHealthCareProviderByID(long HealthCareProviderID);
        Task<IEnumerable<HealthCareProviderDTO>> GetAllHealthCareProviders();
    }
}
