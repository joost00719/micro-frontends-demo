using Microsoft.Extensions.DependencyInjection;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandAloneService
{
    public class PluginDescription : AbstractPlugin
    {
        public PluginDescription(IServiceCollection services)
            : base(services)
        {
            StyleSheets.Add("/_content/StandAloneService/css/color-theme.css");
        }
    }
}
