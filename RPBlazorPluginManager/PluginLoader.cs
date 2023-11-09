using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using RPBlazorPlugin.Core;
using System.IO.Compression;
using System.Xml;

namespace RPBlazorPlugin.Loader
{
    public class PackageRepository
    {
        public IReadOnlyCollection<PluginInfo> LoadedPlugins => _loadedPlugins.AsReadOnly();

        private List<PluginInfo> _loadedPlugins = new List<PluginInfo>();

        private readonly string _wwwRootPath;

        private string GetPluginPath(string pluginName) => Path.Combine(GetPluginBasePath(), pluginName);

        private string GetPluginBasePath() => Path.Combine(_wwwRootPath, "plugins");

        public PackageRepository(string wwwrootFolder)
        {
            this._wwwRootPath = wwwrootFolder;
        }

        public void Clean()
        {
            var path = Path.Combine(_wwwRootPath, "plugins");
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public PluginInfo SavePackage(Microsoft.Extensions.DependencyInjection.IServiceCollection services, ZipArchive nugetPackage, bool disposeZip = true)
        {
            try
            {
                var dllEntry = nugetPackage.Entries.FirstOrDefault(e => e.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase));
                if (dllEntry == null)
                {
                    throw new ArgumentException("No dll found in nuget package");
                }

                using (var fs = dllEntry.Open())
                using (var ms = new System.IO.MemoryStream())
                {
                    fs.CopyTo(ms);
                    ms.Position = 0;
                    var bytes = ms.ToArray();
                    var assembly = System.Reflection.Assembly.Load(bytes);
                    var types = assembly.GetTypes();
                    var pluginType = types.SingleOrDefault(t => typeof(PluginDefinition).IsAssignableFrom(t));
                    if (pluginType == null)
                    {
                        var msg = $"No valid entry point found in nuget package. Missing implementation of abstract type '{typeof(PluginDefinition)}'";
                        throw new InvalidOperationException(msg);
                    }

                    var createMethod = pluginType.GetConstructor(new Type[] { typeof(IServiceCollection) })!;
                    var plugin = (PluginDefinition)createMethod.Invoke(new[] { services })!;

                    var loadedPlugin = new PluginInfo(plugin, assembly);
                    _loadedPlugins.Add(loadedPlugin);

                    SaveAssets(nugetPackage, loadedPlugin);

                    if (false)
                    {
                    }

                    return loadedPlugin;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (disposeZip)
                {
                    nugetPackage?.Dispose();
                }
            }
        }

        private void SaveAssets(ZipArchive archive, PluginInfo plugin)
        {
            var dir = Path.Combine(_wwwRootPath, "plugins", plugin.Name);
            Directory.CreateDirectory(dir);

            var validFormats = new string[] { ".dll", ".pdb", ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".json", ".txt", ".csv" };

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (validFormats.Contains(Path.GetExtension(entry.Name))
                    || entry.Name == "Microsoft.AspNetCore.StaticWebAssets.props") // Static content specification
                {
                    string path = Path.Combine(dir, entry.Name == "Microsoft.AspNetCore.StaticWebAssets.props" ? "assets.xml" : entry.Name);
                    using Stream zipStream = entry.Open();
                    using FileStream fileStream = new FileStream(path, FileMode.Create);
                    zipStream.CopyTo(fileStream);
                }
            }
        }
    }
}
