
namespace SoftwareVerification.WpfClient.Models
{
    /// <summary>
    /// Что отправляем в api post (Id, Result ставит сервер)
    /// </summary>
    public class CreateVerificationObjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime ReceivedAt { get; set; }
        public string? ExpertComment { get; set; }
    }
}

