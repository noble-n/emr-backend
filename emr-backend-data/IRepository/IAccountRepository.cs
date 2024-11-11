using emr_backend_data.RepoPayload;
using emr_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.IRepository
{
    public interface IAccountRepository
    {
        Task<dynamic> CreateUser(CreateUserVM request);
        Task<string> VerifyUser(string Token, string LoggedInWithIPAddress, DateTime DateCreated);
        Task<UserDTO> FindUser(long? UserId, string Email, string AccessToken);
        Task<UserDTO> FindUser(long? UserId);
        Task<UserDTO> FindUser(string Email);
        Task<UserDTO> GetUserByID(long userID);
        Task<UserDTO> GetUserByEmail(string Email);
        Task<IEnumerable<UserDTO>> GetAllUsers();
        Task<string> UpdateRefreshToken(string RefreshToken, long UserId);
        Task<string> ChangePassword(long UserId, string defaultPassword);
        Task<string> LogoutUser(string Email);
        Task<string> LogInUser(string Email);

    }
}
