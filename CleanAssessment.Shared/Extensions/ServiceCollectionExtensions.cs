using CleanAssessment.Shared.Attributes;
using CleanAssessment.Shared.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Shared.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services, ServiceLifetime serviceLifetime)
        where TService : class
        where TImplementation : class, TService
        => Add(services, serviceLifetime, typeof(TService), typeof(TImplementation));
    public static IServiceCollection Add(this IServiceCollection services, ServiceLifetime serviceLifetime, Type serviceType, Type implementationType)
    {
        return serviceLifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentOutOfRangeException($"Not mapping for service lifetime: {nameof(serviceLifetime)}"),
        };
    }

    private static readonly Type _serviceType = typeof(IService);
    public static void AddAllServices(this IServiceCollection services)
    {
        //var allAttributes = ReflectionHelper.AllTypes.Where(_serviceType.IsAssignableFrom);
        //var allImplementations = allAttributes.SelectMany(x => ReflectionHelper.FindTypesByAttribute(x));

        var orderedServices = ServiceHelper.OrderServiceRecords();
    }
}
