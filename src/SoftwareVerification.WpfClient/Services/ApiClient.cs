using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using SoftwareVerification.WpfClient.Models;

namespace SoftwareVerification.WpfClient.Services;

public class ApiClient
{
    private readonly HttpClient _http;

    public ApiClient(string baseUrl)
    {
        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        _http = new HttpClient(handler)
        {
            BaseAddress = new Uri(baseUrl)
        };
    }
    public async Task<List<VerificationObjectDto>> GetAllAsync(string? name = null, string? type = null, string? result = null)
    {
        var query = new List<string>();

        if (!string.IsNullOrWhiteSpace(name))
            query.Add($"name={Uri.EscapeDataString(name)}"); 

        if (!string.IsNullOrWhiteSpace(type))
            query.Add($"type={Uri.EscapeDataString(type)}");

        if (!string.IsNullOrWhiteSpace(result))
            query.Add($"result={Uri.EscapeDataString(result)}");

        var url = "api/verification-objects";
        if (query.Count > 0)
            url += "?" + string.Join("&", query);

        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadFromJsonAsync<List<VerificationObjectDto>>();
        return items ?? new List<VerificationObjectDto>(); //если слева null - вернуть list 
    }

    public async Task<VerificationObjectDto?> GetByIdAsync(int id)
    {
        var response = await _http.GetAsync($"api/verification-objects/{id}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<VerificationObjectDto>();
    }


    public async Task<VerificationObjectDto> CreateAsync(CreateVerificationObjectDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/verification-objects", dto);
        response.EnsureSuccessStatusCode();

        var created = await response.Content.ReadFromJsonAsync<VerificationObjectDto>();
        return created ?? throw new InvalidOperationException("Сервер вернул пустой ответ при создании.");
    }


    public async Task<VerificationObjectDto?> UpdateAsync(int id, UpdateVerificationObjectDto dto)
    {
        var response = await _http.PatchAsJsonAsync($"api/verification-objects/{id}", dto);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<VerificationObjectDto>();
    }
}