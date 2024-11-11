using emr_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.IRepository
{
    public interface IJwtManager
    {
        Task<AuthResponse> GenerateJsonWebToken(AccessUserVM user);
        Task<AuthResponse> RefreshJsonWebToken(long accountId, Claim[] claims);

    }
}
