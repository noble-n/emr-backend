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
    }
}
