using SoftwareVerification.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SoftwareVerification.Domain.Dto
{
    public class CreateVerificationObjectDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string Version { get; set; } = string.Empty;

        [Required]
        public VerificationType Type { get; set; } //Принимаю enum чтобы asp валидировал значения и возвращал 400 на неизвестные

        [Required]
        public DateTime ReceivedAt { get; set; }

        [StringLength(2000)]
        public string? ExpertComment { get; set; }
    }
}
