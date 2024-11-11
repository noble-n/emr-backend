using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.IRepository
{
    public interface IRefreshTokenGenerator
    {
        string GenerateRefreshToken();

    }
}
