using RPBlazorPlugin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
