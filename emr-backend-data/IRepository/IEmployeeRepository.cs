using emr_backend_data.RepoPayload;
using emr_backend_data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.IRepository
{
    public interface IEmployeeRepository
    {
        Task<dynamic> CreateEmployee(CreateEmployeeVM request);
        Task<string> UpdateEmployee(UpdateEmployeeVM request);
        Task<string> DeleteEmployee(long EmployeeID,long deletedByUserId);
        Task<EmployeeDTO> GetEmployeeByID(long EmployeeID);
        Task<EmployeeDTO> GetEmployeeByUserID(long UserID);
        Task<IEnumerable<EmployeeDTO>> GetAllEmployees(long healthcareProviderId);
    }
}
