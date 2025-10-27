using HRCommerceApp.Core.DTOs.Auth;
using HRCommerceApp.Core.Enums;
using HRCommerceApp.Core.Interfaces;
using HRCommerceApp.Core.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace HRCommerceApp.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return new LoginResponseDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = userRoles.ToList()
                    }
                };
            }
            return null;
        }

        public async Task<ApplicationUser> RegisterAsync(LoginDto registerDto, string role)
        {
            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null)
                throw new Exception("User already exists!");

            ApplicationUser user = new()
            {
                Email = registerDto.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDto.Email,
                FirstName = "Admin",
                LastName = "User",
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
                throw new Exception($"User creation failed! Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            await _userManager.AddToRoleAsync(user, role);

            return user;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task SeedRolesAsync()
        {
            if (!await _roleManager.RoleExistsAsync(UserRole.Administrador))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Administrador));

            if (!await _roleManager.RoleExistsAsync(UserRole.Operador))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Operador));
        }

        public async Task SeedAdminUserAsync()
        {
            var adminUser = await _userManager.FindByEmailAsync("admin@hrcommerce.com");
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@hrcommerce.com",
                    Email = "admin@hrcommerce.com",
                    FirstName = "Admin",
                    LastName = "Sistema",
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "Admin123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRole.Administrador);
                }
            }

            var operatorUser = await _userManager.FindByEmailAsync("operador@hrcommerce.com");
            if (operatorUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "operador@hrcommerce.com",
                    Email = "operador@hrcommerce.com",
                    FirstName = "Operador",
                    LastName = "Sistema",
                    CreatedAt = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, "Operador123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRole.Operador);
                }
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}
