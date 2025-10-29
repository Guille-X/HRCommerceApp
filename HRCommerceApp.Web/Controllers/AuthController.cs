using HRCommerceApp.Core.DTOs.Auth;
using HRCommerceApp.Core.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRCommerceApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthWebService _authWebService;

        public AuthController(IAuthWebService authWebService)
        {
            _authWebService = authWebService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto model)
        {
            Console.WriteLine("=== FRONTEND LOGIN START ===");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                Console.WriteLine("Calling AuthWebService (API call)...");
                var result = await _authWebService.LoginAsync(model);

                if (result == null)
                {
                    Console.WriteLine("AuthWebService returned null - invalid credentials");
                    ModelState.AddModelError(string.Empty, "Credenciales inválidas");
                    return View(model);
                }

                Console.WriteLine("AuthWebService returned valid result from API");

                // Crear claims para la cookie
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, result.User.Email),
            new Claim(ClaimTypes.Email, result.User.Email),
            new Claim("FirstName", result.User.FirstName ?? "User"),
            new Claim("LastName", result.User.LastName ?? "Unknown"),
            new Claim("Token", result.Token)
        };

                // Agregar roles
                if (result.User.Roles != null)
                {
                    foreach (var role in result.User.Roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                }

                // USAR IdentityConstants.ApplicationScheme
                var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddHours(3),
                    AllowRefresh = true
                };

                Console.WriteLine(" Signing in with Identity.Application scheme...");
                await HttpContext.SignInAsync(
                    IdentityConstants.ApplicationScheme, 
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                HttpContext.Session.SetString("JWTToken", result.Token);

                Console.WriteLine("LOGIN SUCCESSFUL - Redirecting to Dashboard");
                return RedirectToAction("Dashboard", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" LOGIN EXCEPTION: {ex.Message}");
                Console.WriteLine($" STACK TRACE: {ex.StackTrace}");

                ModelState.AddModelError(string.Empty, "Error durante el proceso de autenticación");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // USAR EL MISMO ESQUEMA PARA LOGOUT
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            HttpContext.Session.Remove("JWTToken");
            return RedirectToAction("Login", "Auth");
        }
    }

    }