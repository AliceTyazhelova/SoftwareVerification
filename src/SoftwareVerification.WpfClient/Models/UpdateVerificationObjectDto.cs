
namespace SoftwareVerification.WpfClient.Models
{
    /// <summary>
    /// То, что отправляем на изменение в API - patch
    /// </summary>
    public class UpdateVerificationObjectDto
    {
        public string Result { get; set; } = string.Empty;
        public string? ExpertComment { get; set; }
    }
}
