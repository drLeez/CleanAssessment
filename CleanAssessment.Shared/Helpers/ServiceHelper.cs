using CleanAssessment.Shared.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Helpers;
public class ServiceHelper
{
    internal static WebApplicationBuilder WebApplicationBuilder;
    public ServiceHelper(WebApplicationBuilder webApplicationBuilder)
    {
        WebApplicationBuilder = webApplicationBuilder;
    }
    internal static IServiceCollection Services => WebApplicationBuilder.Services;
    internal static IConfiguration Configuration => WebApplicationBuilder.Configuration;


    private static readonly Type _optionsConfigurationServiceCollectionExtensionsType = typeof(OptionsConfigurationServiceCollectionExtensions);
    private static MethodInfo? _configureMethod;
    internal static void TryConfigure(string? configurationSection, Type implementationType)
    {
        if (string.IsNullOrWhiteSpace(configurationSection)) return;

        _configureMethod ??= _optionsConfigurationServiceCollectionExtensionsType.GetMethod("Configure"
            , BindingFlags.Static | BindingFlags.Public
            , [typeof(IConfiguration)]
        )?.GetGenericMethodDefinition();

        var section = Configuration.GetSection(configurationSection)
                ?? throw new ArgumentException($"Could not find section {configurationSection} in configuration");

        _configureMethod.MakeGenericMethod([implementationType]).Invoke(null, [Services, section]);
    }
    internal static void TryConfigure<TImplementation>(string? configurationSection)
        => TryConfigure(configurationSection, typeof(TImplementation));

    internal static void RegisterService(Type? serviceType, Type implementationType)
    {
        if (implementationType.IsGenericType)
            throw new Exception($"{implementationType} must NOT be a generic type to be a service");
        var constructor = implementationType.GetConstructors().SingleOrDefault()
            ?? throw new ArgumentException($"{implementationType} must have only ONE constructor to be a valid service type");

        var rawParams = constructor.GetParameters().OrderBy(x => x.Position).Select(x => x.ParameterType);
        HashSet<Type> paramTypes = [];
        foreach (var param in rawParams)
        {
            paramTypes.AddRange(ReflectionHelper.UnfoldTypes(param));
        }
        _records.Add(new(serviceType, implementationType, paramTypes));
    }

    private record ServiceRecord(Type? ServiceType, Type ImplementationType, HashSet<Type> ConstructorParamTypes);
    private static readonly HashSet<ServiceRecord> _records = [];

    public static List<ServiceRecord> OrderServiceRecords()
    {
        List<ServiceRecord> ret = [];
        foreach (var serviceRecord in _records)
        {

        }
    }
}
