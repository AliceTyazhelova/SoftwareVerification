using SoftwareVerification.Domain.Dto;

namespace SoftwareVerification.BlazorServer.Services;

public class VerificationService
{
    private readonly VerificationApiClient _api;

    // Список объектов, который будет читать Razor-страница
    public List<VerificationObjectDto> Objects { get; private set; } = new();

    // Флаги для отображения спиннеров/блокировки кнопок
    public bool IsLoading { get; private set; }
    public bool IsSaving { get; private set; }

    public VerificationService(VerificationApiClient api)
    {
        _api = api;
    }

    // Метод для загрузки списка из API
    public async Task LoadDataAsync()
    {
        IsLoading = true;
        try
        {
            // Вызываем метод из вашего ApiClient
            Objects = await _api.GetAllAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в VerificationService (LoadData): {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Метод для сохранения нового объекта
    public async Task<bool> CreateAsync(CreateVerificationObjectDto dto)
    {
        IsSaving = true;
        try
        {
            var result = await _api.CreateAsync(dto);
            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка в VerificationService (Create): {ex.Message}");
            return false;
        }
        finally
        {
            IsSaving = false;
        }
    }
}