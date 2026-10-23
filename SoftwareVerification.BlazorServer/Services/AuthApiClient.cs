using Microsoft.AspNetCore.Identity.Data;
using SoftwareVerification.Domain.Dto;

namespace SoftwareVerification.BlazorServer.Services
{
    public class AuthApiClient : IAuthApiClient
    {
        private readonly HttpClient _httpClient;

        public AuthApiClient(IHttpClientFactory httpClientFactory)
        {
            // Использование клиента, который настроен на порт 8001
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<LoginResponseDto?> LoginAsync(string username, string password)
        {
            // Отправляем LoginRequest (если он тоже есть в Domain)
            var response = await _httpClient.PostAsJsonAsync("/api/auth/login", new LoginRequestDto
            {
                Username = username,
                Password = password
            });

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            // Читаем LoginResponse
            return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        }
    }

}
