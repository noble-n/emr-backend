using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace emr_backend_data.ViewModel
{
    public class CreatePatientVM
    {
        
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        [JsonIgnore]
        public long HealthCareProviderId { get; set; }
        [JsonIgnore]
        public long CreatedByUserId { get; set; }
    }
    public class UpdatePatientVM
    {
        public long PatientId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        [JsonIgnore]
        public long UpdatedByUserId { get; set; }
    }
}
