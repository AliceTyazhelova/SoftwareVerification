using SoftwareVerification.Domain.Dto;

namespace SoftwareVerification.BlazorServer.Services
{
    public class VerificationApiClient
    {
        private readonly HttpClient _httpClient;

        public VerificationApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }

        /// <summary>
        /// Отправка данных из формы создания нового объекта исследования в api
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> CreateAsync(CreateVerificationObjectDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/verification-objects", dto);
            
            if(response.IsSuccessStatusCode) return true;

            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {response.StatusCode}. {error}");
                return false;
            }
        }

        public async Task<List<VerificationObjectDto>> GetAllAsync()
        {
            // Стучимся в API за списком
            var response = await _httpClient.GetFromJsonAsync<List<VerificationObjectDto>>("/api/verification-objects");
            return response ?? new List<VerificationObjectDto>();
        }
    }
}
