using emr_backend_business.ILogic;
using emr_backend_data.RepoPayload;
using emr_backend_data.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace emr_backend_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel request)
        {
            return Ok(await _authService.Login(request));
        }
        [HttpPost]
        [Route("SuperAdminSignUp")]
        public async Task<IActionResult> SuperAdminSignUp(CreateSuperAdminUserVM request)
        {
            return Ok(await _authService.SuperAdminSignUp(request));
        }
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout(LogoutDTO payload)
        {
            return Ok(await _authService.Logout(payload));
        }
    }
}
