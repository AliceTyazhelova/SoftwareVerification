using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareVerification.WpfClient.Models
{
    /// <summary>
    /// Фильтры
    /// </summary>
    class FilterOptions
    {
        public const string All = "Все";

        public static readonly IReadOnlyList<string> Types = new[]
        {
        All,
        "Software",
        "HardwareSoftwareComplex"
    };

        public static readonly IReadOnlyList<string> Results = new[]
        {
        All,
        "InProgress",
        "Compliant",
        "NonCompliant"
    };
    }
}
