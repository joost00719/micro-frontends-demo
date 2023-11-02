using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RPBlazorPluginManager
{
    public class LoadedPlugin
    {
        public LoadedPlugin(AbstractPlugin plugin, Assembly assembly)
        {
            Plugin = plugin ?? throw new ArgumentNullException(nameof(plugin));
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        public AbstractPlugin Plugin { get; }
        public Assembly Assembly { get; }
    }
}
