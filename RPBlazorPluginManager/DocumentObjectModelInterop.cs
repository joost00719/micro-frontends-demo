using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace RPBlazorPluginManager
{
    /// <summary>
    /// DOM interop
    /// </summary>
    public class DocumentObjectModelInterop
    {
        private readonly IJSRuntime _jsRuntime;

        public DocumentObjectModelInterop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        }

        /// <summary>
        /// Includes a css stylesheet in the web application.
        /// </summary>
        /// <param name="href">The url of the stylesheet.</param>
        /// <returns>A task which can be awaited.</returns>
        public async Task IncludeStylesheet(string href)
        {
            await _jsRuntime.InvokeVoidAsync("includeStylesheet", href);
        }

        /// <summary>
        /// Includes a js script in the web application
        /// </summary>
        /// <param name="src">The url of the script</param>
        /// <returns>A task which can be awaited</returns>
        public async Task IncludeScript(string src)
        {
            await _jsRuntime.InvokeVoidAsync("includeScript", src);
        }

        public async Task LoadPlugingAssets(string pluginName)
        {
            await _jsRuntime.InvokeVoidAsync("loadPluginAssets", pluginName);
        }
    }
}
