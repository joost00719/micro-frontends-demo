using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPBlazorPluginManager
{
    public class PluginInfo
    {
        public PluginInfo(AbstractPlugin plugin, Assembly assembly)
        {
            Plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        public AbstractPlugin Plugin { get; }

        public Assembly Assembly { get; }

        public string Name => Assembly.GetName().Name!;

        public List<PluginAsset> Assets { get; set; } = new List<PluginAsset>();
    }

    public class PluginAsset
    {
        public string SourceType { get; set; }

        public string SourceId { get; set; }

        public string ContentRoot { get; set; }

        public string BasePath { get; set; }

        public string RelativePath { get; set; }

        public string AssetKind { get; set; }

        public string AssetMode { get; set; }

        public string AssetRole { get; set; }

        public string AssetTraitName { get; set; }

        public string AssetTraitValue { get; set; }

        public string OriginalItemSpec { get; set; }

        public bool IsJavaScript() => RelativePath?.EndsWith(".js", StringComparison.OrdinalIgnoreCase) ?? false;

        public bool IsCss() => RelativePath?.EndsWith(".css", StringComparison.OrdinalIgnoreCase) ?? false;

        public string GetPath() => Path.Combine(RelativePath, BasePath);
    }
}
