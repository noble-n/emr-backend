using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_common.Communication
{
    public class ExecutedResult<T>
    {
        public string responseCode { get; set; }
        public string responseMessage { get; set; }
        public T data { get; set; }

    }
}
