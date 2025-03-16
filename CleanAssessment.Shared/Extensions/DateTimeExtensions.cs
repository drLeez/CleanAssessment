using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public static int ToDateId(this DateTime dateTime)
        {
            return int.Parse(dateTime.ToString("yyyyMMdd"));
        }
    }
}
