using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace RPBlazorPluginManager
{
    public class PluginLoader
    {
        public IReadOnlyCollection<LoadedPlugin> LoadedPlugins => _loadedPlugins.AsReadOnly();

        private List<LoadedPlugin> _loadedPlugins = new List<LoadedPlugin>();

        private readonly string pluginFolderPath;

        public PluginLoader(string pluginFolderPath)
        {
            this.pluginFolderPath = pluginFolderPath;
        }

        public async Task<PluginLoader> LoadPlugins(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            var files = System.IO.Directory.GetFiles(pluginFolderPath, "*.nupkg");

            foreach (var file in files)
            {
                await this.Load(file, services);
            }

            return this;
        }

        private async Task Load(string file, Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            using (var zip = new System.IO.Compression.ZipArchive(System.IO.File.OpenRead(file)))
            {
                var dllEntry = zip.Entries.FirstOrDefault(e => e.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));
                if (dllEntry != null)
                {
                    using (var fs = dllEntry.Open())
                    using (var ms = new System.IO.MemoryStream())
                    {
                        await fs.CopyToAsync(ms);
                        ms.Position = 0;
                        var bytes = ms.ToArray();
                        var assembly = System.Reflection.Assembly.Load(bytes);
                        var types = assembly.GetTypes();
                        var pluginType = types.FirstOrDefault(t => typeof(Shared.AbstractPlugin).IsAssignableFrom(t));
                        if (pluginType != null)
                        {
                            var createMethod = pluginType.GetConstructor(new Type[] { typeof(IServiceCollection) })!;
                            var plugin = (AbstractPlugin)createMethod.Invoke(new[] { services })!;

                            _loadedPlugins.Add(new LoadedPlugin(plugin, assembly));
                        }
                    }
                }
            }
        }
    }
}
