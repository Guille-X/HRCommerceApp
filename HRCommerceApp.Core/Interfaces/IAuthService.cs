using HRCommerceApp.Core.DTOs.Auth;
using HRCommerceApp.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Core.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<ApplicationUser> RegisterAsync(LoginDto registerDto, string role);
        Task<bool> UserExistsAsync(string email);
        Task SeedRolesAsync();
        Task SeedAdminUserAsync();
    }
}
