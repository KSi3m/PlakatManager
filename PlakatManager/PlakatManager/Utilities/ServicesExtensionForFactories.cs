using System.Runtime.CompilerServices;

namespace PlakatManager.Utilities
{
    public static class ServicesExtensionForFactories
    {

        public static void RegisterFactories(this IServiceCollection services)
        {
            services.AddSingleton<ElectionItemFactoryRegistry>();
            services.AddSingleton<LEDFactory>();
            services.AddSingleton<PosterFactory>();
            services.AddSingleton<BillboardFactory>();
        }


    }
}
