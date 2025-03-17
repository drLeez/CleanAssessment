using CleanAssessment.Shared.Enums;

namespace CleanAssessment.Helpers
{
    public static class ServiceHelper
    {
        public static void AddSecondLayerInterface<T>(this IServiceCollection services, ServiceType serviceType)
        {
            var baseInterface = typeof(T);
            if (!baseInterface.IsInterface) throw new ArgumentException($"Type param T ({baseInterface.Name}) must be an interface");

            var types = baseInterface
                .Assembly
                .GetExportedTypes()
                .Where(x => x.IsClass && !x.IsAbstract)
                .Select(x => new
                {
                    Service = x.GetInterface($"I{x.Name}"),
                    Implementation = x
                })
                .Where(x => x.Service != null);

            foreach (var type in types)
            {
                if (baseInterface.IsAssignableFrom(type.Service))
                {
                    switch (serviceType)
                    {
                        case ServiceType.Transient:
                            services.AddTransient(type.Service, type.Implementation);
                            break;
                        case ServiceType.Scoped:
                            services.AddScoped(type.Service, type.Implementation);
                            break;
                        case ServiceType.Singleton:
                            services.AddSingleton(type.Service, type.Implementation);
                            break;
                    }
                    
                }
            }
        }
    }
}
