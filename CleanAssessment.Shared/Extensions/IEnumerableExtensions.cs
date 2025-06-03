using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }

        public static string Comma<T>(this IEnumerable<T> enumerable, Func<T, string?>? selector = null)
        {
            selector ??= x => x?.ToString();
            return string.Join(", ", enumerable.Select(selector));
        }
    }
}
