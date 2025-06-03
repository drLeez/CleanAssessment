using CleanAssessment.Shared.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Helpers;
public static class ReflectionHelper
{
    public static IEnumerable<Type> AllTypes => AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes());

    public static IEnumerable<Type> FindAllImplementationsOf(Type interfaceType, TypeFlags typeFlags = TypeFlags.None)
    {
        if (!interfaceType.IsInterface)
            throw new ArgumentException($"{nameof(interfaceType)} must be an interface");
        
        if (interfaceType.IsGenericType)
            interfaceType = interfaceType.GetGenericTypeDefinition();

        foreach (var type in AllTypes)
        {
            if (!interfaceType.IsAssignableFrom(type)) continue;
            if (typeFlags.HasFlag(TypeFlags.Class) && !type.IsClass) continue;
            if (typeFlags.HasFlag(TypeFlags.Interface) && !type.IsInterface) continue;
            if (typeFlags.HasFlag(TypeFlags.Enum) && !type.IsEnum) continue;
            yield return type;
        }
    }

    private static readonly Type _attributeType = typeof(Attribute);
    public static IEnumerable<Type> FindTypesByAttribute(Type attributeType, TypeFlags typeFlags = TypeFlags.None)
    {
        if (!_attributeType.IsAssignableFrom(attributeType))
            throw new ArgumentException($"{attributeType.Name} does not extend Attribute");
        
        if (attributeType.IsGenericType)
            attributeType = attributeType.GetGenericTypeDefinition();

        foreach (var type in AllTypes)
        {
            if (!type.CustomAttributes.Any(x => x.AttributeType == attributeType)) continue;
            if (typeFlags.HasFlag(TypeFlags.Class) && !type.IsClass) continue;
            if (typeFlags.HasFlag(TypeFlags.Interface) && !type.IsInterface) continue;
            if (typeFlags.HasFlag(TypeFlags.Enum) && !type.IsEnum) continue;
            yield return type;
        }
    }
    public static IEnumerable<Type> FindTypesByAttribute<TAttribute>(TypeFlags typeFlags = TypeFlags.None)
        where TAttribute : Attribute
        => FindTypesByAttribute(typeof(TAttribute), typeFlags);

    public static HashSet<Type> UnfoldTypes(Type type, HashSet<Type>? alreadySeen = null)
    {
        alreadySeen ??= [];
        if (alreadySeen.Contains(type)) return alreadySeen;
        if (!type.IsGenericType)
        {
            alreadySeen.Add(type);
            return alreadySeen;
        }

        alreadySeen.Add(type.GetGenericTypeDefinition());
        foreach (var t in type.GetGenericArguments())
        {
            UnfoldTypes(t, alreadySeen);
        }
        return alreadySeen;
    }
}
