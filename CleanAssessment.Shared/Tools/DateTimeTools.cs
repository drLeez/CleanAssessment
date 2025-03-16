using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Tools
{
    public static class DateTimeTools
    {
        public static DateTime FromDateId(int dateId)
        {
            return DateTime.ParseExact(dateId.ToString(), "yyyyMMdd", null);
        }
    }
}
