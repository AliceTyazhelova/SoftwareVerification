using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace SoftwareVerification.BlazorServer.Services;

public class NotificationClientService : IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private readonly IJSRuntime _js;
    private readonly NavigationManager _nav;
    private readonly NotificationService _notificationService;

    public event Action? BellUpdated;

    public EventCallback OnBellUpdate { get; set; }

    public NotificationClientService(IJSRuntime js, NavigationManager nav, NotificationService notificationService)
    {
        _js = js;
        _nav = nav;
        _notificationService = notificationService;
    }

    public async Task StartAsync()
    {
        // Если уже подключены — не подключаемся повторно
        if (_hubConnection is not null && _hubConnection.State == HubConnectionState.Connected)
            return;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_nav.ToAbsoluteUri("/notificationHub"))
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string>("ReceiveNotification", async (message) =>
        {
            try
            {
                // Вызываем JS для показа Toast
                await _js.InvokeVoidAsync("showToast", message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка JS: {ex.Message}");
            }
        });

        _hubConnection.On<int>("UpdateBell", (count) =>
        {
            BellUpdated?.Invoke();
        });

        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine($"=== HUB ПОДКЛЮЧЕН. STATE: {_hubConnection.State} ===");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"=== ОШИБКА ПОДКЛЮЧЕНИЯ: {ex.Message} ===");
        }
    }

    public async Task SendNotificationAsync(string message)
    {
        if (_hubConnection is not null && _hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.SendAsync("SendNotification", message);
        }
        else
        {
            Console.WriteLine("=== ОШИБКА: HUB НЕ ПОДКЛЮЧЕН ===");
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}