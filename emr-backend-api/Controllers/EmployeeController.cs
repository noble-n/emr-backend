using emr_backend_business.ILogic;
using emr_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace emr_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        [Route("UpdateEmployee")]
        [Authorize]
        public async Task<IActionResult> UpdateEmployee( UpdateEmployeeVM request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _employeeService.UpdateEmployee(request, accessToken, RemoteIpAddress));

        }
        [HttpPost]
        [Route("DeleteEmployee")]
        [Authorize]
        public async Task<IActionResult> DeleteEmployee(long EmployeeID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _employeeService.DeleteEmployee(EmployeeID, accessToken, RemoteIpAddress));

        }

        [HttpGet]
        [Route("GetEmployeeByID")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeByID(long EmployeeID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _employeeService.GetEmployeeByID(EmployeeID, accessToken, RemoteIpAddress));

        }

        [HttpGet]
        [Route("GetEmployeeByUserID")]
        [Authorize]
        public async Task<IActionResult> GetEmployeeByUserID(long UserID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _employeeService.GetEmployeeByUserID(UserID, accessToken, RemoteIpAddress));

        }

        [HttpGet]
        [Route("GetAllEmployees")]
        [Authorize]
        public async Task<IActionResult> GetAllEmployees()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _employeeService.GetAllEmployee(accessToken, RemoteIpAddress));

        }
    }
}
