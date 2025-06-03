using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Tools;
public static class ExTools
{
    public static ArgumentException InvalidArgs(object? arg, [CallerArgumentExpression("arg")] string? argName = default)
    {
        return new ArgumentException($"{argName} is invalid: {{ {arg} }}");
    }
    public static ArgumentException InvalidArgs(object?[] args, [CallerArgumentExpression("args")] string? argNames = default)
    {
        var names = argNames?[1..^1].Split(", ");
        var sb = new StringBuilder();
        for (int i = 0; i < args.Length; i++)
        {
            sb.Append($"{names?[i]} is invalid: {{ {args[i]} }}; ");
        }
        return new ArgumentException(sb.ToString());
    }
}
