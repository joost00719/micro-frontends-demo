using Microsoft.Extensions.DependencyInjection;
using RPBlazorPlugin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandAloneService
{
    public class PluginDescription : PluginDefinition
    {
        public PluginDescription(IServiceCollection services)
            : base(services)
        {
    //< link href = "RoboPharma.Web.Demo.Server.styles.css" rel = "stylesheet" />

            // TODO: Figure out which css file to add
            StyleSheets.Add("StandAloneService.styles.css");
        }
    }
}
