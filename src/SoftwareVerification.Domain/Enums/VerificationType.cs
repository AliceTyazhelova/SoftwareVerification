using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareVerification.Domain.Enums
{
    /// <summary>
    /// Тип объекта проверки
    /// </summary>
    public enum VerificationType
    {
        //Программное обеспечение
        Software = 1,

        //Программно-аппаратный комплекс
        HardwareSoftwareComplex = 2
    }
}
