using System.Windows.Input;
using SoftwareVerification.WpfClient.Commands;
using SoftwareVerification.WpfClient.Common;
using SoftwareVerification.WpfClient.Models;
using SoftwareVerification.WpfClient.Services;

namespace SoftwareVerification.WpfClient.ViewModels;

public class CreateWindowViewModel : ObservableObject
{
    private readonly ApiClient _api;

    public CreateWindowViewModel(ApiClient api)
    {
        _api = api;

        SaveCommand = new AsyncRelayCommand(SaveAsync, CanSave);
        CancelCommand = new RelayCommand(Cancel);
    }

    #region Поля формы

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    private string _version = string.Empty;
    public string Version
    {
        get => _version;
        set => SetProperty(ref _version, value);
    }

    //Для привязки ItemsSource="{Binding TypeOptions}"
    public IReadOnlyList<string> TypeOptions { get; } = new[]
    {
        "Software",
        "HardwareSoftwareComplex"
    };

    private string _type = "Software";
    public string Type
    {
        get => _type;
        set => SetProperty(ref _type, value);
    }

    private DateTime? _receivedAt = DateTime.Today;
    public DateTime? ReceivedAt
    {
        get => _receivedAt;
        set => SetProperty(ref _receivedAt, value);
    }

    private string? _expertComment;
    public string? ExpertComment
    {
        get => _expertComment;
        set => SetProperty(ref _expertComment, value);
    }

    #endregion

    #region Состояние

    private string _statusMessage = string.Empty;
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    private bool _isSaving;
    public bool IsSaving
    {
        get => _isSaving;
        set => SetProperty(ref _isSaving, value);
    }

    public bool? DialogResult { get; private set; }

    #endregion

    #region Команды

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    // Событие для закрытия окна — View подпишется.
    public event Action? RequestClose;

    #endregion

    #region Реализация

    private bool CanSave()
        => !IsSaving
           && !string.IsNullOrWhiteSpace(Name)
           && !string.IsNullOrWhiteSpace(Version)
           && ReceivedAt.HasValue;

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            StatusMessage = "Введите название.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Version))
        {
            StatusMessage = "Введите версию.";
            return;
        }

        if (!ReceivedAt.HasValue)
        {
            StatusMessage = "Выберите дату поступления.";
            return;
        }

        IsSaving = true;
        StatusMessage = "Сохранение...";

        try
        {
            var dto = new CreateVerificationObjectDto
            {
                Name = Name.Trim(),
                Version = Version.Trim(),
                Type = Type,
                ReceivedAt = DateTime.SpecifyKind(ReceivedAt.Value, DateTimeKind.Utc),
                ExpertComment = string.IsNullOrWhiteSpace(ExpertComment) ? null : ExpertComment.Trim() 
                //если пользователь оставил пусто или ввёл только пробелы, отправляем null. Иначе — обрезанную строку
            };

            await _api.CreateAsync(dto);

            DialogResult = true;
            RequestClose?.Invoke();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
        finally
        {
            IsSaving = false;
        }
    }

    private void Cancel()
    {
        DialogResult = false;
        RequestClose?.Invoke();
    }

    #endregion
}