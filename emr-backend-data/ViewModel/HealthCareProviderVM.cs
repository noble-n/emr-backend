using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace emr_backend_data.ViewModel
{
    public class CreateHealthCareProviderVM
    {
        public string HealthCareProviderName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public long CreatedByUserId { get; set; }
    }
    public class UpdateHealthCareProviderVM
    {
        public long HealthCareProviderId { get; set; }
        public string HealthCareProviderName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }

        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public long UpdatedByUserId { get; set; }
    }
}
