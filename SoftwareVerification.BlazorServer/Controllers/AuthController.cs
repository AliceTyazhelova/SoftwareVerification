using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SoftwareVerification.BlazorServer.Services;

namespace SoftwareVerification.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IAuthApiClient _authClient;

        public AuthController(IAuthApiClient authClient)
        {
            _authClient = authClient;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
        {
            var userData = await _authClient.LoginAsync(username, password);

            if (userData != null)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, userData.Username) };
                foreach (var role in userData.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties { IsPersistent = true });

                if (userData.Roles.Contains("Admin"))
                    return Redirect("/verifications");

                return Redirect("/");
            }

            return Redirect("/login?error=1");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/login");
        }
    }
}

