using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace RPBlazorPlugin.Loader
{
    public static class RPBlazorPluginManagerHelper
    {
        /// <summary>
        /// Adds the ability to load plugins from a folder . The folder should contain .nupkg files.
        /// </summary>
        /// <remarks>
        /// Two additional steps are required to get this working: <br />
        /// 1. In your App.Razor, set the Router.AdditionalAssemblies property with the assemblies from the <see cref="PluginAssetLoader.GetLoadedAssemblies"/> <br />
        /// 2. In your App.Razor, call <see cref="PluginAssetLoader.LoadAssets()"/> on the first render. <br />
        /// <see cref="PluginAssetLoader"/> can be injected.
        /// </remarks>
        /// <param name="services">The serviceprovider to add services to</param>
        /// <param name="settings">The settings for the plugin loader.</param>
        /// <returns>The service collection</returns>
        public static IServiceCollection AddPlugins(this IServiceCollection services, PluginManagerSettings settings)
        {
            var pluginLoader = new PackageRepository(settings.WWWRootFolderPath!);
            pluginLoader.Clean(); // clean up previous plugins

            var packages = Directory.EnumerateFiles(settings.PackedPluginsFolderPath!, "*.nupkg");

            foreach (var package in packages)
            {
                pluginLoader.SavePackage(services, new FileInfo(package));
            }

            services.AddScoped<DocumentObjectModelInterop>();
            services.AddHttpClient<PluginAssetLoader, PluginAssetLoader>((sp, client) => client.BaseAddress = new Uri(sp.GetRequiredService<NavigationManager>().BaseUri));
            services.AddTransient<PluginAssetLoader>();

            return services.AddSingleton(_ => pluginLoader);
        }
    }
}
