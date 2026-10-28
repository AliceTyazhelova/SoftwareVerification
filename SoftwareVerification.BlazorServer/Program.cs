using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SoftwareVerification.BlazorServer.Components;
using SoftwareVerification.BlazorServer.Hubs;
using SoftwareVerification.BlazorServer.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:8001"); 
});

builder.Services.AddSingleton<NotificationService>();
builder.Services.AddScoped<NotificationClientService>();
//builder.Services.AddScoped<VerificationService>();

builder.Services.AddScoped<IAuthApiClient, AuthApiClient>();
builder.Services.AddScoped<VerificationApiClient>();

builder.Services.AddSignalR();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "SoftwareVerification.Auth"; // Имя Cookie
        options.LoginPath = "/login"; // Путь перенаправления, если пользователь не аутентифицирован
        options.AccessDeniedPath = "/login";  // Путь перенаправления, если нет прав доступа
        options.ExpireTimeSpan = TimeSpan.FromHours(24); // Время истечения
        options.SlidingExpiration = true; // Продление при активности
    });

builder.Services.AddControllers();
// 3. Добавление сервисов авторизации (для проверки ролей)
builder.Services.AddAuthorization();

// 4. Каскадное состояние аутентификации (обязательно для использования AuthorizeView и т.д. в компонентах)
builder.Services.AddCascadingAuthenticationState();

// В интерактивном режиме Blazor Server HttpContext недоступен напрямую, при необходимости получаем информацию через этот интерфейс
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// использовать UseAuthentication и UseAuthorization перед UseAntiforgery
app.UseAuthentication(); // Включает промежуточное ПО аутентификации (считывает Cookie и устанавливает User)
app.UseAuthorization();  // Включает промежуточное ПО авторизации

app.UseAntiforgery(); // Blazor Server требует защиты от подделки запросов

app.MapControllers();
// Подключение конечных точек Razor Components
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(); // Включает интерактивный режим сервера

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
