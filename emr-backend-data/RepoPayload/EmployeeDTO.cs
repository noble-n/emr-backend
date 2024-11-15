using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.RepoPayload
{
    public class EmployeeDTO
    {
        public long EmployeeId { get; set; }
        public long HealthCareProviderId { get; set; }
        public long UserId { get; set; }
        public long DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public long RoleId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public long UpdatedByUserId { get; set; }
        public DateTime DateUpdated { get; set; }
        public long DeletedByUserId { get; set; }
        public DateTime DateDeleted { get; set; }
        public bool IsDeleted { get; set; }
    }
}
