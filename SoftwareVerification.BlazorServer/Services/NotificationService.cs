namespace SoftwareVerification.BlazorServer.Services;

public class NotificationItem
{
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public bool IsRead { get; set; } = false;
}

public class NotificationService
{
    // Один экземпляр на всё приложение
    public Guid InstanceId { get; } = Guid.NewGuid();
    public List<NotificationItem> Notifications { get; } = new();
    public Action? OnUpdate { get; set; }

    public void AddNotification(string message)
    {
        Notifications.Insert(0, new NotificationItem { Message = message });
        NotifyStateChanged();
    }

    public void NotifyStateChanged()
    {
        Console.WriteLine($"=== SERVICE: Вызываю OnChange. Подписчиков: {OnUpdate?.GetInvocationList().Length ?? 0} ===");
        OnUpdate?.Invoke();
    }
    public int UnreadCount => Notifications.Count(n => !n.IsRead);

    public void MarkAllAsRead()
    {
        foreach (var n in Notifications) n.IsRead = true;
        NotifyStateChanged();
    }
}