using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.RepoPayload
{
    public class UserDTO
    {
        public long UserId { get; set; }
        public long HealthCareProviderId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }

        public bool IsLogin { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public bool IsDeactivated { get; set; }
        public string DeactivatedComment { get; set; }
        public long? DeactivatedByUserId { get; set; }
        public DateTime? DateDeactivated { get; set; }

        public string RefreshToken { get; set; }
        public string Token { get; set; }

        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsActive { get; set; }

    }
}
