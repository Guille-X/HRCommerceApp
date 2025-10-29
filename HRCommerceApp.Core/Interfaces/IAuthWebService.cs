using HRCommerceApp.Core.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCommerceApp.Core.Interfaces
{
    public interface IAuthWebService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> UserExistsAsync(string email);
    }
}
