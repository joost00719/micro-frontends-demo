using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace RPBlazorPlugin.Core
{
    public abstract class PluginDefinition
    {
        public PluginDefinition(IServiceCollection services)
        {
            services.AddSingleton(typeof(PluginDefinition), this);
            services.AddSingleton(GetType(), this);
        }

        public List<string> Scripts { get; } = new List<string>();

        public List<string> StyleSheets { get; } = new List<string>();

        public List<IPluginPageInfo> Pages { get; } = new List<IPluginPageInfo>();
    }
}