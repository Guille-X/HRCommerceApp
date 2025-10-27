using Microsoft.AspNetCore.Mvc;
using HRCommerceApp.Core.Interfaces;
using HRCommerceApp.Core.DTOs.Auth;
using HRCommerceApp.Core.Enums;

namespace HRCommerceApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                if (result == null)
                    return Unauthorized(new { message = "Credenciales inválidas" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] LoginDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto, UserRole.Administrador);
                return Ok(new { message = "Usuario administrador creado exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("seed-data")]
        public async Task<IActionResult> SeedData()
        {
            try
            {
                await _authService.SeedRolesAsync();
                await _authService.SeedAdminUserAsync();
                return Ok(new { message = "Datos iniciales creados exitosamente" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}