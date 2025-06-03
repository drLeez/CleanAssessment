using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Extensions;
public static class HashSetExtensions
{
    public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T?> items)
    {
        foreach (var item in items)
        {
            if (item != null) hashSet.Add(item);
        }
    }
    public static void AddRange<T>(this HashSet<T> hashSet, params T?[] items)
        => hashSet.AddRange(items.ToList());
}
