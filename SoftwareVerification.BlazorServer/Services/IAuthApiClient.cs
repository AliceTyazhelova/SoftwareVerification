using Microsoft.AspNetCore.Identity.Data;
using SoftwareVerification.Domain.Dto;

namespace SoftwareVerification.BlazorServer.Services
{
    public interface IAuthApiClient
    {
        Task<LoginResponseDto?> LoginAsync(string username, string password);
    }
}
