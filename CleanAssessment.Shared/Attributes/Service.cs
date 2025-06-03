using CleanAssessment.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Options;
using System.Diagnostics;
using CleanAssessment.Shared.Helpers;
using CleanAssessment.Shared.Enums;

namespace CleanAssessment.Shared.Attributes;

public interface IService
{
    public ServiceLifetime Lifetime { get; }
}

[AttributeUsage(AttributeTargets.Class)]
public class Service<TService, TImplementation> : Attribute, IService
    where TService : class
    where TImplementation : class, TService
{
    public ServiceLifetime Lifetime { get; }

    public Service(ServiceLifetime serviceLifetime, string? configurationSection = null)
    {
        Lifetime = serviceLifetime;

        ServiceHelper.RegisterService(typeof(TService), typeof(TImplementation));
        ServiceHelper.TryConfigure<TImplementation>(configurationSection);
        ServiceHelper.Services.Add<TService, TImplementation>(Lifetime);
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class Service : Attribute, IService
{
    public ServiceLifetime Lifetime { get; }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public Service(ServiceLifetime serviceLifetime, string? configurationSection = null)
    {
        Lifetime = serviceLifetime;

        StackFrame frame = new(1);
        var implementationType = frame.GetMethod()?.DeclaringType
            ?? throw new Exception($"Could not infer implementation type");

        var types = ReflectionHelper.AllTypes.Where(x => x.Name == $"I{implementationType.Name}" && x.IsInterface);
        var serviceType = types.SingleOrDefault()
            ?? throw new Exception($"Cannot find single interface for implementation type {implementationType.Name};"
            + $" found instead: [{types.Comma(x => x.Name)}]");

        ServiceHelper.RegisterService(serviceType, implementationType);
        ServiceHelper.TryConfigure(configurationSection, implementationType);
        ServiceHelper.Services.Add(Lifetime, serviceType, implementationType);
    }
}

