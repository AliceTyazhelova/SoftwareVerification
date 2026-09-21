using Microsoft.EntityFrameworkCore;
using SoftwareVerification.Domain.Entities;
using SoftwareVerification.Domain.Enums;
using SoftwareVerification.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()); //парсит строку из JSON в enum
    });

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

//Автоприменение миграций при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Если таблица пустая — добавляем тестовые данные.
    if (!db.VerificationObjects.Any())
    {
        for (int g = 1; g <= 30; g++)
        {
            var names = new[]
            {
                "Система управления доступом Альфа", "Система управления доступом Бэта", "Офисный пакет", "СУБД", "CRM",
                "Видеофиксация", "СКУД", "Мониторинг", "Терминал сбора данных", "Среда разработки"
            };

            var name = $"Образец {g} - {names[(g - 1) % names.Length]}";
            var version = $"{(1 + (g - 1) % 5)}.{((g * 7) % 10)}.{((g * 3) % 20)}";
            var type = g % 3 == 0
                ? VerificationType.HardwareSoftwareComplex
                : VerificationType.Software;
            var receivedAt = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-g), DateTimeKind.Utc);
            var result = (VerificationResult)((g - 1) % 3);
            var comment = g % 4 == 0 ? null : $"Комментарий эксперта для образца {g}";

            var obj = new VerificationObject(name, version, type, receivedAt);
            obj.SetResult(result);
            obj.SetExpertComment(comment);

            db.VerificationObjects.Add(obj);
        }

        db.SaveChanges();
    }
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();