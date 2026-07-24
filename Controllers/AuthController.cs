using Microsoft.AspNetCore.Mvc;
using VIRTUAL_CLASSE_FORMATION.Dtos;
using VIRTUAL_CLASSE_FORMATION.Services;

namespace VIRTUAL_CLASSE_FORMATION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
        {
            try
            {
                var result = await authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
        {
            var result = await authService.LoginAsync(dto);
            if (result == null)
                return Unauthorized(new { message = "Email ou mot de passe incorrect." });

            return Ok(result);
        }
    }
}
