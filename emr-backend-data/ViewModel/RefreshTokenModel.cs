using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace emr_backend_data.ViewModel
{
    public class RefreshTokenModel
    {
        [Required]
        public string JwtToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
