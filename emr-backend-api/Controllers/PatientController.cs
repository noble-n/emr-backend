using emr_backend_business.ILogic;
using emr_backend_business.Logic;
using emr_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace emr_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientService _patientService;
        public PatientController(ILogger<PatientController> logger, IPatientService patientService)
        {
            _logger = logger;
            _patientService = patientService;
        }
        [HttpPost]
        [Route("CreatePatient")]
        [Authorize]
        public async Task<IActionResult> CreatePatient(CreatePatientVM request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _patientService.CreatePatient(request, accessToken, RemoteIpAddress));

        }

        [HttpPost]
        [Route("UpdatePatient")]
        [Authorize]
        public async Task<IActionResult> UpdatePatient(UpdatePatientVM request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _patientService.UpdatePatient(request, accessToken, RemoteIpAddress));

        }

        [HttpPost]
        [Route("DeletePatient")]
        [Authorize]
        public async Task<IActionResult> DeletePatient(long PatientID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _patientService.DeletePatient(PatientID, accessToken, RemoteIpAddress));

        }

        [HttpGet]
        [Route("GetPatientByID")]
        [Authorize]
        public async Task<IActionResult> GetPatientByID(long PatientID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _patientService.GetPatientByID(PatientID, accessToken, RemoteIpAddress));

        }

        [HttpGet]
        [Route("GetAllPatients")]
        [Authorize]
        public async Task<IActionResult> GetAllPatients()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _patientService.GetAllPatient(accessToken, RemoteIpAddress));

        }
    }
}
