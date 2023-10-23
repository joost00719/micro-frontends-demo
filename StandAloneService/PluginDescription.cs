using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandAloneService
{
    public class PluginDescription : IPlugin
    {
        public PluginDescription()
        {
            StyleSheets = new List<string>()
            {
                "/_content/StandAloneService/css/color-theme.css"
            };
        }

        public List<string> Scripts { get; set; }

        public List<string> StyleSheets { get; set; }

        public string Name { get; set; }

        public static IPlugin CreateInstance()
        {
            return new PluginDescription();
        }
    }
}
