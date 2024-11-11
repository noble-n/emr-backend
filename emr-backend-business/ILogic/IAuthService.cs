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
    public interface IAuthService
    {
        //Task<AuthResponse> GenerateJsonWebToken(AccountDTO user);
        //Task<RefreshTokenResponse> RefreshToken(RefreshTokenModel refresh, string ipAddress, string port);
        Task<ExecutedResult<LoginResponse>> Login(LoginModel payload);
        Task<ExecutedResult<string>> SuperAdminSignUp(CreateSuperAdminUserVM payload);
        Task<ExecutedResult<string>> Logout(LogoutDTO payload);
        Task<ExecutedResult<string>> ChangePassword(ChangePasswordDTO payload/*, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort*/);
        Task<ExecutedResult<UserDTO>> CheckUserAccess(string AccessToken, string IpAddress);



    }
}
