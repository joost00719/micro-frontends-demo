using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPBlazorPlugin.Loader
{
    public class PluginManagerSettings
    {
        public required string? WWWRootFolderPath { get; set; }

        public required string? PackedPluginsFolderPath { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(WWWRootFolderPath) && !string.IsNullOrEmpty(PackedPluginsFolderPath);
        }
    }
}
