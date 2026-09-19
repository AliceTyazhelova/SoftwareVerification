namespace SoftwareVerification.Domain.Enums
{
    /// <summary>
    /// Результат проверки объекта
    /// </summary>
    public enum VerificationResult
    {
        //Проверка ещё не завершена
        InProgress = 0,

        //Cоответствует требованиям
        Compliant = 1,

        //Не соответствует требованиям
        NonCompliant = 2
    }
}
