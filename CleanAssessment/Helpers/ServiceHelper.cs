namespace CleanAssessment.Helpers
{
    public static class ServiceHelper
    {
        public static void AddSecondLayerInterface<T>(this IServiceCollection services)
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
                    services.AddTransient(type.Service, type.Implementation);
                }
            }
        }
    }
}
