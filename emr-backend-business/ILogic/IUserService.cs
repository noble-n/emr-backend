using emr_backend_common.Communication;
using emr_backend_data.RepoPayload;
using emr_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_business.ILogic
{
    public interface IUserService
    {
        Task<ExecutedResult<string>> CreateUser(CreateEmployeeVM payload, string AccessToken, string RemoteIpAddress);
        Task<ExecutedResult<UserDTO>> GetUserById(long UserId, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<UserDTO>> GetUserByEmail(string email, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);
        Task<ExecutedResult<IEnumerable<UserDTO>>> GetAllUsers( string route, string AccessKey, IEnumerable<Claim> claim, string RemoteIpAddress, string RemotePort);


    }
}
