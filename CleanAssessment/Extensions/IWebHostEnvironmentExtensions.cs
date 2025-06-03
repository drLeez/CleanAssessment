using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Extensions
{
    public static class IWebHostEnvironmentExtensions
    {
        public static bool IsLocal(this IWebHostEnvironment? webHostEnvironment)
        {
            return webHostEnvironment?.IsEnvironment("Local") ?? false;
        }
    }
}
