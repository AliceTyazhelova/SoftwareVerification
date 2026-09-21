using SoftwareVerification.Domain.Enums;

namespace SoftwareVerification.Domain.Entities
{
    public class VerificationObject
    {
        public int Id { get; private set; } 

        //Название продукта
        public string Name { get; private set; } = string.Empty;

        //Версия образца
        public string Version { get; private set; } = string.Empty;

        //ПО или Программно-аппаратный комплекс
        public VerificationType Type { get; private  set; }

        //Дата поступления образца в лабораторию 
        public DateTime ReceivedAt { get; private set; }

        //Текущий результат проверки, при создании статус "в работе"
        public VerificationResult Result { get; private set; } = VerificationResult.InProgress;

        //Комментарий от эксперта, не обязательное поле
        public string? ExpertComment { get; private set; }

        //Для EF
        private VerificationObject() { }

        public VerificationObject(string name, string version, VerificationType type, DateTime receivedAt, string? expertComment = null)
        {
            Name = name;
            Version = version;
            Type = type;
            ReceivedAt = receivedAt;
            ExpertComment = expertComment;
        }

        public void SetExpertComment(string? comment)
        {
            ExpertComment = comment;
        }
        public void SetResult(VerificationResult result)
        {
            Result = result;
        }
    }
}
