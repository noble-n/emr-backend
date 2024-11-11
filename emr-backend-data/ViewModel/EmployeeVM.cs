using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace emr_backend_data.ViewModel
{
    public class CreateEmployeeVM
    {
        public long HealthCareProviderId { get; set; }
        [JsonIgnore]
        public long UserId { get; set; }
        public long DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public long RoleId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
