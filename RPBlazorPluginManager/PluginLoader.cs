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
        private readonly string _wwwRootPath;
        private List<PluginInfo> _loadedPlugins = new List<PluginInfo>();
        private AssemblyLoadContext _dependencies = new AssemblyLoadContext("plugins", false);

        public IReadOnlyCollection<PluginInfo> LoadedPlugins => _loadedPlugins.AsReadOnly();

        public PackageRepository(string wwwrootFolder)
        {
            _wwwRootPath = wwwrootFolder;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            _dependencies.LoadFromAssemblyName(typeof(PluginDefinition).Assembly.GetName());
        }

        public void Clean()
        {
            var path = Path.Combine(_wwwRootPath, "plugins");
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public PluginInfo SavePackage(IServiceCollection services, FileInfo nugetFileInfo)
        {
            if (!nugetFileInfo.Exists)
            {
                throw new ArgumentException($"File '{nugetFileInfo.FullName}' does not exist");
            }

            using (var nugetPackage = ZipFile.OpenRead(nugetFileInfo.FullName))
            {
                ConstructorInfo? createMethod = null;
                Assembly? createAssembly = null;

                var dlls = nugetPackage.Entries.Where(e => e.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)).ToList();

                foreach (var dllEntry in dlls)
                {
                    using (var fs = dllEntry.Open())
                    using (var ms = new System.IO.MemoryStream())
                    {
                        fs.CopyTo(ms);
                        ms.Position = 0;
                        var bytes = ms.ToArray();
                        if (IsPluginAssembly(_dependencies.LoadFromStream(new MemoryStream(bytes))))
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
        }

        private bool IsPluginAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes();

            var name = typeof(PluginDefinition).Assembly.GetName().ToString();
            var coreAsm = _dependencies.Assemblies.SingleOrDefault(x => x.GetName().ToString() == name);
            var pluginSubType = coreAsm.GetType(typeof(PluginDefinition).FullName!)!;
            var pluginType = types.FirstOrDefault(t => t.IsSubclassOf(pluginSubType));

            return pluginType != null;
        }

        private Assembly? CurrentDomain_AssemblyResolve(object? sender, ResolveEventArgs args)
        {
            return _dependencies.Assemblies.FirstOrDefault(asm => asm.GetName().ToString() == args.Name);
        }

        private void SaveAssets(ZipArchive archive, PluginInfo plugin)
        {
            var dir = Path.Combine(_wwwRootPath, "plugins", plugin.Name);
            Directory.CreateDirectory(dir);

            var validFormats = new string[] { ".dll", ".pdb", ".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".json", ".txt", ".csv" };

            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (validFormats.Contains(Path.GetExtension(entry.Name))
                    || entry.Name == "Microsoft.AspNetCore.StaticWebAssets.props")
                {
                    string path = Path.Combine(dir, entry.Name == "Microsoft.AspNetCore.StaticWebAssets.props" ? "assets.xml" : entry.Name);
                    using (Stream zipStream = entry.Open())
                    using (FileStream fileStream = new FileStream(path, FileMode.Create))
                    {
                        zipStream.CopyTo(fileStream);
                    }
                }
            }
        }
    }
}
