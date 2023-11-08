using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using Shared;
using System.IO.Compression;
using System.Xml;

namespace RPBlazorPluginManager
{
    public class PackageRepository
    {
        public IReadOnlyCollection<LoadedPlugin> LoadedPlugins => _loadedPlugins.AsReadOnly();

        private List<LoadedPlugin> _loadedPlugins = new List<LoadedPlugin>();

        private readonly string _wwwRootPath;

        private string GetPluginPath(string pluginName) => Path.Combine(GetPluginBasePath(), pluginName);

        private string GetPluginBasePath() => Path.Combine(_wwwRootPath, "plugins");

        public PackageRepository(string wwwrootFolder)
        {
            this._wwwRootPath = wwwrootFolder;
        }

        public async Task Clean()
        {
            var path = Path.Combine(_wwwRootPath, "plugins");
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public async Task<LoadedPlugin> SavePackage(Microsoft.Extensions.DependencyInjection.IServiceCollection services, ZipArchive nugetPackage, bool disposeZip = true)
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
                    await fs.CopyToAsync(ms);
                    ms.Position = 0;
                    var bytes = ms.ToArray();
                    var assembly = System.Reflection.Assembly.Load(bytes);
                    var types = assembly.GetTypes();
                    var pluginType = types.SingleOrDefault(t => typeof(Shared.AbstractPlugin).IsAssignableFrom(t));
                    if (pluginType == null)
                    {
                        var msg = $"No valid entry point found in nuget package. Missing implementation of abstract type '{typeof(AbstractPlugin)}'";
                        throw new InvalidOperationException(msg);
                    }

                    var createMethod = pluginType.GetConstructor(new Type[] { typeof(IServiceCollection) })!;
                    var plugin = (AbstractPlugin)createMethod.Invoke(new[] { services })!;

                    var loadedPlugin = new LoadedPlugin(plugin, assembly);
                    _loadedPlugins.Add(loadedPlugin);

                    await SaveAssets(nugetPackage, loadedPlugin);

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

        private async Task SaveAssets(ZipArchive archive, LoadedPlugin plugin)
        {
            var dir = Path.Combine(_wwwRootPath, "plugins", plugin.Name);
            Directory.CreateDirectory(dir);

            var validFormats = new string[] { ".dll", ".pdb", ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".json", ".txt", ".csv" };

            // Read all
            var assetsFile = archive.Entries.Single(entry => entry.Name.Contains("Microsoft.AspNetCore.StaticWebAssets.props"));
            ProcessProperties(assetsFile, plugin);

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (validFormats.Contains(Path.GetExtension(entry.Name))
                    || entry.Name == "Microsoft.AspNetCore.StaticWebAssets.props") // Static content specification
                {
                    string path = Path.Combine(dir, entry.Name == "Microsoft.AspNetCore.StaticWebAssets.props" ? "assets.xml" : entry.Name);
                    using Stream zipStream = entry.Open();
                    using FileStream fileStream = new FileStream(path, FileMode.Create);
                    await zipStream.CopyToAsync(fileStream);
                }
            }
        }

        /// <summary>
        /// Returns the base-path of the plugin
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="plugin"></param>
        /// <returns></returns>
        private void ProcessProperties(ZipArchiveEntry entry, LoadedPlugin plugin)
        {
            string xmlStr;
            // Read the file to string
            using (var stream = entry.Open())
            using (var reader = new StreamReader(stream))
            {
                xmlStr = reader.ReadToEnd();
            }

            var assetsList = new XmlDocument();
            assetsList.LoadXml(xmlStr);

            foreach (XmlNode assetXml in assetsList.GetElementsByTagName("StaticWebAsset"))
            {
                // node to PluginAsset
                var json = JsonConvert.SerializeXmlNode(assetXml, Newtonsoft.Json.Formatting.Indented);
                var asset = JsonConvert.DeserializeAnonymousType(json, new { StaticWebAsset = new PluginAsset() }).StaticWebAsset;

                plugin.Assets.Add(asset);
            }

            // TODO: Make the path relative to the plugin folder
            // Load it in the app.razor file
            // ???
            // profit
        }
    }
}
