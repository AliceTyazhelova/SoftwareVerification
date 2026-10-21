using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SoftwareVerification.BlazorServer.Components;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:8001"); 
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "SoftwareVerification.Auth"; // Имя Cookie
        options.LoginPath = "/login"; // Путь перенаправления, если пользователь не аутентифицирован
        options.AccessDeniedPath = "/access-denied"; // Путь перенаправления, если нет прав доступа
        options.ExpireTimeSpan = TimeSpan.FromHours(24); // Время истечения
        options.SlidingExpiration = true; // Продление при активности
    });

// 3. Добавление сервисов авторизации (для проверки ролей)
builder.Services.AddAuthorization();

// 4. Каскадное состояние аутентификации (обязательно для использования AuthorizeView и т.д. в компонентах)
builder.Services.AddCascadingAuthenticationState();

// 5. (Опционально, но рекомендуется) Добавление HttpContextAccessor
// В интерактивном режиме Blazor Server HttpContext недоступен напрямую, при необходимости получаем информацию через этот интерфейс
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.MapPost("/api/login", async (HttpContext context) =>
{
    // Читаем данные из формы
    var form = await context.Request.ReadFormAsync();
    var username = form["username"].ToString();
    var password = form["password"].ToString();

    // В реальном проекте: проверка через ваш сервис из Infrastructure
    if (username == "admin" && password == "123")
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties { IsPersistent = true };

        // ✅ Устанавливаем Cookie в рамках HTTP-запроса — это работает!
        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return Results.Redirect("/"); // Перенаправляем на главную
    }

    return Results.Redirect("/login?error=1"); // Возвращаемся с ошибкой
}).DisableAntiforgery(); // Отключаем антифоржери для простоты (в продакшене настройте правильно)

app.UseHttpsRedirection();
app.UseStaticFiles();

// важно: использовать UseAuthentication и UseAuthorization перед UseAntiforgery
app.UseAuthentication(); // Включает промежуточное ПО аутентификации (считывает Cookie и устанавливает User)
app.UseAuthorization();  // Включает промежуточное ПО авторизации

app.UseAntiforgery(); // Blazor Server требует защиты от подделки запросов

// Подключение конечных точек Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Включает интерактивный режим сервера

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
