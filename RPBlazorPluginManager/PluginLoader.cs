using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using RPBlazorPlugin.Core;
using System.Collections.Generic;
using System.IO.Compression;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Loader;

namespace RPBlazorPlugin.Loader
{
    public class PackageRepository
    {
        public IReadOnlyCollection<PluginInfo> LoadedPlugins => _loadedPlugins.AsReadOnly();

        private List<PluginInfo> _loadedPlugins = new List<PluginInfo>();

        private readonly string _wwwRootPath;

        private string GetPluginPath(string pluginName) => Path.Combine(GetPluginBasePath(), pluginName);

        private string GetPluginBasePath() => Path.Combine(_wwwRootPath, "plugins");

        private AssemblyLoadContext _dependancies = new AssemblyLoadContext("plugins", false);

        public PackageRepository(string wwwrootFolder)
        {
            this._wwwRootPath = wwwrootFolder;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            _dependancies.LoadFromAssemblyName(typeof(PluginDefinition).Assembly.GetName());
        }

        public void Clean()
        {
            var path = Path.Combine(_wwwRootPath, "plugins");
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public PluginInfo SavePackage(Microsoft.Extensions.DependencyInjection.IServiceCollection services, FileInfo nugetFileInfo, bool disposeZip = true)
        {
            if (!nugetFileInfo.Exists)
            {
                throw new ArgumentException($"File '{nugetFileInfo.FullName}' does not exist");
            }

            var nugetPackage = ZipFile.OpenRead(nugetFileInfo.FullName);
            // var assemblies = new List<(string name, byte[] assembly)>(nugetPackage.Entries.Count);

            ConstructorInfo? createMethod = null;
            Assembly? createAssembly = null;
            try
            {
                var dlls = nugetPackage.Entries.Where(e => e.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)).ToList();

                foreach (var dllEntry in dlls)
                {
                    using (var fs = dllEntry.Open())
                    using (var ms = new System.IO.MemoryStream())
                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;
                        var bytes = ms.ToArray();
                        // assemblies.Add((dllEntry.Name, bytes));
                        if (IsPluginAssembly(_dependancies.LoadFromStream(new MemoryStream(bytes))))
                        {
                            createAssembly = Assembly.Load(bytes);
                            createMethod = createAssembly
                                .GetTypes()
                                .SingleOrDefault(x => x.IsAssignableTo(typeof(PluginDefinition)))!
                                .GetConstructor(new[] { typeof(IServiceCollection) });
                        }
                    }
                }

                var plugin = (PluginDefinition)createMethod.Invoke(new[] { services })!;

                var loadedPlugin = new PluginInfo(plugin, createAssembly!);
                _loadedPlugins.Add(loadedPlugin);

                SaveAssets(nugetPackage, loadedPlugin);

                return loadedPlugin;
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

        private bool IsPluginAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes();

            // _dependancies.LoadFromAssemblyName(typeof(PluginDefinition).Assembly.GetName());
            var name = typeof(PluginDefinition).Assembly.GetName().ToString();
            var coreAsm = _dependancies.Assemblies.SingleOrDefault(x => x.GetName().ToString() == name);
            var pluginSubType = coreAsm.GetType(typeof(PluginDefinition).FullName!)!;
            // get all types of which the base class is of type PluginDefinition
            var pluginType = types.FirstOrDefault(t => t.IsSubclassOf(pluginSubType));

            return pluginType != null;
        }

        private Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            return _dependancies.Assemblies.FirstOrDefault(asm => asm.GetName().ToString() == (args.Name));
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
