using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace emr_backend_data.ViewModel
{
    public class CreateUserVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public string PasswordHash { get; set; }
        public long RoleId { get; set; }
        public DateTime DateCreated { get; set; }
        public long HealthCareProviderId { get; set; }

    }
    public class CreateSuperAdminUserVM
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public long RoleId { get; set; }
        public DateTime DateCreated { get; set; }

        [JsonIgnore]
        public long HealthCareProviderId { get; set; }

    }
}
