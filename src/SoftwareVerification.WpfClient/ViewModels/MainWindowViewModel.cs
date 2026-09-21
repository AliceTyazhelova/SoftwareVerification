using System.Collections.ObjectModel;
using System.Windows.Input;
using SoftwareVerification.WpfClient.Commands;
using SoftwareVerification.WpfClient.Common;
using SoftwareVerification.WpfClient.Models;
using SoftwareVerification.WpfClient.Services;
using SoftwareVerification.WpfClient.Views;

namespace SoftwareVerification.WpfClient.ViewModels;

public class MainWindowViewModel : ObservableObject
{

    private readonly ApiClient _api;

    private bool _isReloading;

    private string? _originalResult;
    private string? _originalExpertComment;

    public MainWindowViewModel(ApiClient api)
    {
        _api = api;

        LoadCommand = new AsyncRelayCommand(LoadAsync);
        CreateCommand = new RelayCommand(Create);
        SaveEditCommand = new AsyncRelayCommand(SaveEditAsync, () => HasSelection);
        CancelEditCommand = new RelayCommand(CancelEdit, () => HasUnsavedChanges);
        _ = LoadAsync();
    }

    #region Данные

    public ObservableCollection<VerificationObjectDto> Items { get; } = new();

    private VerificationObjectDto? _selectedItem;
    public VerificationObjectDto? SelectedItem
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem == value)
                return;

            if (!_isReloading && HasUnsavedChanges)
            {
                var answer = System.Windows.MessageBox.Show(
                    "Есть несохранённые данные. Продолжить без сохранения?",
                    "Подтверждение",
                    System.Windows.MessageBoxButton.YesNo,
                    System.Windows.MessageBoxImage.Warning);

                if (answer != System.Windows.MessageBoxResult.Yes)
                {
                    OnPropertyChanged(nameof(SelectedItem));
                    return;
                }
            }

            _selectedItem = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasSelection));
            LoadEditFieldsFromSelected();
        }
    }

    public bool HasSelection => SelectedItem != null;

    #endregion

    #region Фильтры

    public IReadOnlyList<string> TypeOptions => FilterOptions.Types;
    public IReadOnlyList<string> ResultOptions => FilterOptions.Results;

    private string _searchName = string.Empty;
    public string SearchName
    {
        get => _searchName;
        set => SetProperty(ref _searchName, value);
    }

    private string _filterType = FilterOptions.All;
    public string FilterType
    {
        get => _filterType;
        set => SetProperty(ref _filterType, value);
    }

    private string _filterResult = FilterOptions.All;
    public string FilterResult
    {
        get => _filterResult;
        set => SetProperty(ref _filterResult, value);
    }

    #endregion

    
    #region Панель редактирования

    private IReadOnlyList<string> _editResultOptions = Array.Empty<string>();
    public IReadOnlyList<string> EditResultOptions
    {
        get => _editResultOptions;
        private set => SetProperty(ref _editResultOptions, value);
    }

    private string _editResult = "InProgress";
    public string EditResult
    {
        get => _editResult;
        set
        {
            if (SetProperty(ref _editResult, value))
                OnPropertyChanged(nameof(HasUnsavedChanges));
        }
    }

    private string? _editExpertComment;
    public string? EditExpertComment
    {
        get => _editExpertComment;
        set
        {
            if (SetProperty(ref _editExpertComment, value))
                OnPropertyChanged(nameof(HasUnsavedChanges));
        }
    }

    public bool HasUnsavedChanges
    {
        get
        {
            if (SelectedItem == null)
                return false;

            var resultChanged = _editResult != _originalResult;
            var commentChanged =
                (_editExpertComment ?? string.Empty) != (_originalExpertComment ?? string.Empty);

            return resultChanged || commentChanged;
        }
    }

    private void LoadEditFieldsFromSelected()
    {
        if (SelectedItem == null)
        {
            EditResultOptions = Array.Empty<string>();
            _editResult = "InProgress";
            _editExpertComment = null;
            _originalResult = null;
            _originalExpertComment = null;
        }
        else
        {
            var current = SelectedItem.Result;

            EditResultOptions = current == "InProgress"
                ? new[] { "InProgress", "Compliant", "NonCompliant" }
                : new[] { "Compliant", "NonCompliant" };

            _editResult = current;
            _editExpertComment = SelectedItem.ExpertComment;
            _originalResult = current;
            _originalExpertComment = SelectedItem.ExpertComment;
        }

        OnPropertyChanged(nameof(EditResult));
        OnPropertyChanged(nameof(EditExpertComment));
        OnPropertyChanged(nameof(HasUnsavedChanges));
    }

    #endregion

    #region Состояние UI

    private bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    private string _statusMessage = "Готов";
    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    #endregion

    #region Команды

    public ICommand LoadCommand { get; }
    public ICommand CreateCommand { get; }
    public ICommand SaveEditCommand { get; }
    public ICommand CancelEditCommand { get; }

    #endregion

    #region Реализация команд

    private async Task LoadAsync()
    {
        IsLoading = true;
        StatusMessage = "Загрузка...";

        try
        {
            var name = string.IsNullOrWhiteSpace(SearchName) ? null : SearchName;
            var type = FilterType == FilterOptions.All ? null : FilterType;
            var result = FilterResult == FilterOptions.All ? null : FilterResult;

            var items = await _api.GetAllAsync(name, type, result);

            _isReloading = true;

            Items.Clear();
            foreach (var item in items)
                Items.Add(item);

            SelectedItem = null;

            _isReloading = false;

            StatusMessage = $"Загружено: {Items.Count}";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка загрузки: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void Create()
    {
        var vm = new CreateWindowViewModel(_api);
        var window = new CreateWindow
        {
            DataContext = vm,
            Owner = System.Windows.Application.Current.MainWindow
        };

        if (window.ShowDialog() == true)
        {
            _ = LoadAsync();
        }
    }

    private async Task SaveEditAsync()
    {
        if (SelectedItem == null) return;

        try
        {
            var dto = new UpdateVerificationObjectDto
            {
                Result = _editResult,
                ExpertComment = string.IsNullOrWhiteSpace(_editExpertComment)
                    ? null
                    : _editExpertComment.Trim()
            };

            var updated = await _api.UpdateAsync(SelectedItem.Id, dto);

            if (updated == null)
            {
                StatusMessage = "Объект не найден на сервере.";
                return;
            }

            _originalResult = updated.Result;
            _originalExpertComment = updated.ExpertComment;

            OnPropertyChanged(nameof(HasUnsavedChanges));

            StatusMessage = "Сохранено.";

            await LoadAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка сохранения: {ex.Message}";
        }
    }

    private void CancelEdit()
    {
        LoadEditFieldsFromSelected();
        StatusMessage = "Изменения сброшены.";
    }

    #endregion
}