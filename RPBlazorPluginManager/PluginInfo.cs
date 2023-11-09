using RPBlazorPlugin.Core;
using System.Reflection;

namespace RPBlazorPlugin.Loader
{
    public class PluginInfo
    {
        public PluginInfo(PluginDefinition plugin, Assembly assembly)
        {
            Plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        public PluginDefinition Plugin { get; }

        public Assembly Assembly { get; }

        public string Name => Assembly.GetName().Name!;
    }
}
