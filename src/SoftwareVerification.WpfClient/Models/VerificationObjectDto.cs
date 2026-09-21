namespace SoftwareVerification.WpfClient.Models

{
    /// <summary>
    /// Что приходит из API 
    /// </summary>
    public class VerificationObjectDto
    {
        public int Id { get; set; }

        //Название продукта
        public string Name { get; set; } = string.Empty;

        //Версия образца
        public string Version { get; set; } = string.Empty;

        //ПО или Программно-аппаратный комплекс
        public string Type { get; set; } = string.Empty; 

        //Дата поступления образца в лабораторию 
        public DateTime ReceivedAt { get; set; }

        //Текущий результат проверки
        public string Result { get; set; } = string.Empty; //На стороне api это уже строки, поэтому не enum

        //Комментарий от эксперта
        public string? ExpertComment { get; set; }
    }
}
