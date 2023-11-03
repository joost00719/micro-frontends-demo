using Microsoft.Extensions.DependencyInjection;
using Shared;
using System.IO.Compression;
using System.Xml;

namespace RPBlazorPluginManager
{
    public class PluginLoader
    {
        public IReadOnlyCollection<LoadedPlugin> LoadedPlugins => _loadedPlugins.AsReadOnly();

        private List<LoadedPlugin> _loadedPlugins = new List<LoadedPlugin>();

        private readonly string pluginFolderPath;

        public PluginLoader(string wwwrootFolder)
        {
            this.pluginFolderPath = wwwrootFolder;
        }

        public async Task<LoadedPlugin> LoadPlugin(Microsoft.Extensions.DependencyInjection.IServiceCollection services, ZipArchive nugetPackage, bool disposeZip = true)
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

                    await SaveNuget(nugetPackage, Path.Combine(pluginFolderPath, "live-plugins"), loadedPlugin);

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

        private async Task SaveNuget(ZipArchive archive, string folder, LoadedPlugin plugin)
        {
            Directory.CreateDirectory(folder);

            var validFormats = new string[] { ".dll", ".pdb", ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".json", ".txt", ".csv" };

            // Read all 
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (validFormats.Contains(Path.GetExtension(entry.Name))
                    /*|| entry.Name == "Microsoft.AspNetCore.StaticWebAssets.props"*/) // Static content specification
                {
                    string path = Path.Combine(folder, entry.Name);
                    using Stream zipStream = entry.Open();
                    using FileStream fileStream = new FileStream(path, FileMode.Create);
                    await zipStream.CopyToAsync(fileStream);
                }

                if (entry.Name == "Microsoft.AspNetCore.StaticWebAssets.props")
                {
                    ProcessProperties(entry, plugin);
                }
            }
        }

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

            foreach (XmlNode asset in assetsList.GetElementsByTagName("StaticWebAsset"))
            {
                var content = asset.SelectSingleNode("RelativePath")?.InnerText!;
                if (content.EndsWith(".js"))
                {
                    plugin.JS.Add(content);
                }
                else if (content.EndsWith(".css"))
                {
                    plugin.CSS.Add(content);
                }
            }

            // TODO: Make the path relative to the plugin folder
            // Load it in the app.razor file
            // ???
            // profit
        }
    }
}
