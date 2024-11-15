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
    public interface IEmployeeService
    {
        Task<ExecutedResult<string>> UpdateEmployee(UpdateEmployeeVM request, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<string>> DeleteEmployee(long EmployeeID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<EmployeeDTO>> GetEmployeeByID(long EmployeeID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<EmployeeDTO>> GetEmployeeByUserID(long UserID, string AccessKey, string RemoteIpAddress);
        Task<ExecutedResult<IEnumerable<EmployeeDTO>>> GetAllEmployee(string AccessKey, string RemoteIpAddress);
    }
}
