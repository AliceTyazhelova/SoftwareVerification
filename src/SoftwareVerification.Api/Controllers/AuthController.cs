using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoftwareVerification.Domain.Dto;
using SoftwareVerification.Domain.Entities;
using SoftwareVerification.Domain.Enums;
using SoftwareVerification.Infrastructure.Data;

namespace SoftwareVerification.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Это сгенерирует путь /api/Auth (без учета регистра)
    public class AuthController:ControllerBase
    {
        private readonly AppDbContext _db;

        public AuthController(AppDbContext db)
        {
            _db = db;
        }
        // SoftwareVerification.Api/Controllers/AuthController.cs
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _db.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Login == request.Username);

            if (user == null || user.PasswordHash != request.Password)
                return Unauthorized();

            // Используем наш новый DTO
            var response = new LoginResponseDto
            {
                Username = user.Login,
                Roles = user.Roles.Select(r => r.Name).ToList()
            };

            return Ok(response);
        }
    }
}
