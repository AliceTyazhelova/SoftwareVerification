using Microsoft.AspNetCore.SignalR;
using SoftwareVerification.BlazorServer.Services;

namespace SoftwareVerification.BlazorServer.Hubs;

public class NotificationHub : Hub
{
    private readonly NotificationService _notificationService;

    public NotificationHub(NotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task SendNotification(string message)
    {
        // 1. Добавляем в общий список (для колокольчика)
        _notificationService.AddNotification(message);
        
        // 2. Рассылаем всем клиентам (для Toast)
        await Clients.All.SendAsync("ReceiveNotification", message);

        // 3. Рассылаем сигнал для колокольчика
        await Clients.All.SendAsync("UpdateBell", _notificationService.Notifications.Count);
    }
}