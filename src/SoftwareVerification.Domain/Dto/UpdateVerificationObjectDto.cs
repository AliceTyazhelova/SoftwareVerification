using SoftwareVerification.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SoftwareVerification.Domain.Dto
{
    public class UpdateVerificationObjectDto
    {
        [Required]
        public VerificationResult Result { get; set; } //Принимаю enum чтобы asp валидировал значения и возвращал 400 на неизвестные

        [StringLength(2000)]
        public string? ExpertComment { get; set; }
    }
}
