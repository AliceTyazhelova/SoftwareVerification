using SoftwareVerification.Domain.Enums;

namespace SoftwareVerification.Api.Dto
{
    /// <summary>
    /// Что отдаем клиенту
    /// </summary>
    public class VerificationObjectDto
    {
        public int Id { get; set; }

        //Название продукта
        public string Name { get; set; } = string.Empty;

        //Версия образца
        public string Version { get; set; } = string.Empty;

        //ПО или Программно-аппаратный комплекс
        public string Type { get; set; } = string.Empty; // Чтобы уходила строка, а не число

        //Дата поступления образца в лабораторию 
        public DateTime ReceivedAt { get; set; }

        //Текущий результат проверки
        public string Result { get; set; } = string.Empty; // InProgress вместо 0

        //Комментарий от эксперта
        public string? ExpertComment { get; set; }
    }
}
