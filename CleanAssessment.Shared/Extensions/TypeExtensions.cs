using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Extensions;
public static class TypeExtensions
{
    private static readonly Type _attributeType = typeof(Attribute);
    public static bool IsAttribute(this Type type) => _attributeType.IsAssignableFrom(type);
}
