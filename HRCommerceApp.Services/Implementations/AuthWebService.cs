using HRCommerceApp.Core.DTOs.Auth;
using HRCommerceApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HRCommerceApp.Services.Implementations
{
    public class AuthWebService : IAuthWebService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuthWebService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            // Configurar base URL de la API
            var apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:7004";
            _httpClient.BaseAddress = new Uri(apiBaseUrl);
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(loginDto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<LoginResponseDto>(
                        responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" AuthWebService error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            // Implementación si es necesaria
            return await Task.FromResult(false);
        }
    }
}
