using ElectionMaterialManager.Entities;
using System.Runtime.CompilerServices;

namespace ElectionMaterialManager.Utilities
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
