using emr_backend_business.ILogic;
using emr_backend_business.Logic;
using emr_backend_data.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace emr_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCareProviderController : ControllerBase
    {
        private readonly ILogger<HealthCareProviderController> _logger;
        private readonly IHealthCareProviderService _healthCareProviderService;
        public HealthCareProviderController(ILogger<HealthCareProviderController> logger, IHealthCareProviderService healthCareProviderService)
        {
            _logger = logger;
            _healthCareProviderService = healthCareProviderService;
        }

        [HttpPost]
        [Route("CreateHealthCareProvider")]
        [Authorize]
        public async Task<IActionResult> CreateHealthCareProvider(CreateHealthCareProviderVM request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _healthCareProviderService.CreateHealthCareProvider(request, accessToken, RemoteIpAddress));

        }

        [HttpPost]
        [Route("UpdateHealthCareProvider")]
        [Authorize]
        public async Task<IActionResult> UpdateHealthCareProvider(UpdateHealthCareProviderVM request)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _healthCareProviderService.UpdateHealthCareProvider(request, accessToken, RemoteIpAddress));

        }

        [HttpPost]
        [Route("DeleteHealthCareProvider")]
        [Authorize]
        public async Task<IActionResult> DeleteHealthCareProvider(long HealthCareProviderID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _healthCareProviderService.DeleteHealthCareProvider(HealthCareProviderID, accessToken, RemoteIpAddress));

        }

        [HttpGet]
        [Route("GetHealthCareProviderByID")]
        [Authorize]
        public async Task<IActionResult> GetHealthCareProviderByID(long HealthCareProviderID)
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _healthCareProviderService.GetHealthCareProviderByID(HealthCareProviderID, accessToken, RemoteIpAddress));

        }

        [HttpGet]
        [Route("GetAllHealthCareProviders")]
        [Authorize]
        public async Task<IActionResult> GetAllHealthCareProviders()
        {
            var RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            var accessToken = Request.Headers["Authorization"];
            accessToken = accessToken.ToString().Replace("bearer", "").Trim();
            return Ok(await _healthCareProviderService.GetAllHealthCareProvider(accessToken, RemoteIpAddress));

        }
    }
}
