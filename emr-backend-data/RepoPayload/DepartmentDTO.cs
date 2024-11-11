using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.RepoPayload
{
    public class DepartmentDTO
    {
        public long DepartmentID { get; set; }
        public long HealthCareProviderId { get; set; }
        public string DepartmentName { get; set; }
    }
}
